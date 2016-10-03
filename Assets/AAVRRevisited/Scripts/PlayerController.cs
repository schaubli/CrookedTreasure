using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public GameObject playerPrefab;
    [HideInInspector]
    public GameObject playerFigure;
	private Tile playerTile;
	private Tile oldPlayerTile; //Tile the player was last
    private Tile rootTile;
    public int playerStartHealth;
	public int playerMaxHealth;
	[HideInInspector]
	public bool isPlayerMovable = true;
	public AnimationCurve rotateAnimation;
	public IEnumerator animationCoroutine;

    private TriggerType triggerType = TriggerType.VR_TRIGGER;
	#if ! UNITY_EDITOR_OSX
    private TransitionManager mTransitionManager;
	#endif
    public enum TriggerType
    {
        VR_TRIGGER,
        AR_TRIGGER
    }

    // Use this for initialization
    public void Initiate (Tile rootTile) {
		instance = this;
		playerFigure = (GameObject) Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		playerFigure.transform.SetParent(this.transform);
		playerFigure.transform.SetAsFirstSibling();
		oldPlayerTile = rootTile;
		this.playerTile = rootTile;
        this.rootTile = rootTile;

		#if ! UNITY_EDITOR_OSX
			mTransitionManager = FindObjectOfType<TransitionManager>();
		#endif
    }
	
	public void MovePlayerToTile(Tile tile) {
		oldPlayerTile = this.playerTile;
		playerTile = tile;
		if(playerFigure==null) {
			Debug.LogError("Make Sure there is not another Player active in the scene");
		}
		
		//Rotate toward new tile
		Quaternion rotation =  Quaternion.FromToRotation(Vector3.forward, tile.transform.localPosition-oldPlayerTile.transform.localPosition);
		animationCoroutine = AnimateRotation(playerFigure.transform, Mathf.RoundToInt(rotation.eulerAngles.y-playerFigure.transform.localRotation.eulerAngles.y), tile.transform.localPosition);
		StartCoroutine(animationCoroutine);
		
		//Check for Enemies and Islands
		List<Tile> neighbours = tile.GetNeighbourTiles();
		int islandcount = 0;
		int monstercount = 0;
		foreach(Tile t in neighbours) {
			if(t.Environment.type == EnvironmentType.Island){
				islandcount++;
			} else if(t.Environment.type == EnvironmentType.Monster){
				monstercount++;
			}
		}
		if(islandcount>0 || monstercount>0) {
            if (tile != this.rootTile)
            {
                StartVRMode(islandcount, monstercount);
            }
		}
        Player.Instance.removeHealth(10);
	}

	private void StartVRMode(int islandCount, int monsterCount) {
        //Start VR Mode and show the correct amount of islands and monsters
        bool goingBackToAR = (triggerType == TriggerType.AR_TRIGGER);
		#if ! UNITY_EDITOR_OSX
    		mTransitionManager.Play(goingBackToAR);
		#endif
    }

		
	

	private IEnumerator AnimateRotation(Transform startTransform, int degrees, Vector3 newPosition) {
		float startdegrees = startTransform.localRotation.eulerAngles.y;
		float duration = 1f;
		float time = 0;
		while( time<duration && degrees != 0) {
			time += Time.deltaTime;
			Vector3 newRot = startTransform.localRotation.eulerAngles;
			newRot.y = startdegrees + degrees*this.rotateAnimation.Evaluate(time/duration);
			playerFigure.transform.localRotation = Quaternion.Euler(newRot);
			yield return new WaitForEndOfFrame();
		}
		// Move Player to new Position
		playerFigure.transform.localPosition = newPosition;
		playerFigure.GetComponent<Animator>().Play("MoveForward");
		Invoke("EnablePlayerMovement",1.01f);
	}

	private void EnablePlayerMovement() {
		this.isPlayerMovable = true;
	}
	
	private static PlayerController instance;
	public static PlayerController Instance { 
		get {
			if(instance == null)
			{
				GameObject obj = new GameObject();
				instance = (PlayerController) obj.AddComponent( typeof(PlayerController));
			}
    		return instance;
    	}
	}

}
