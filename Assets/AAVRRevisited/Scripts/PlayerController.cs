using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public GameObject playerPrefab;
    [HideInInspector]
    public GameObject playerFigure;
	public Tile playerTile;
	private Tile oldPlayerTile; //Tile the player was last
    private Tile rootTile;
    public int playerStartHealth;
	public int playerMaxHealth;
	[HideInInspector]
	public bool isPlayerMovable = true;
	private bool isAnimationPlaying = false;
	public AnimationCurve rotateAnimation;
	public IEnumerator animationCoroutine;

    public GameObject vrHandlerGameobject;
    private VRhandler vrHandler;

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
		if(vrHandlerGameobject != null) {
       		vrHandler = vrHandlerGameobject.GetComponent<VRhandler>();
		}
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
		animationCoroutine = AnimateRotation(playerFigure.transform, Mathf.RoundToInt(rotation.eulerAngles.y-playerFigure.transform.localRotation.eulerAngles.y), tile.transform.localPosition, tile);
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

        //Player.Instance.removeHealth(10);
	}

	private void StartVRMode(int islandCount, int monsterCount) {
        bool goingBackToAR = (triggerType == TriggerType.AR_TRIGGER);
        #if !UNITY_EDITOR_OSX

        if (vrHandler != null)
        {
            if (monsterCount > 0 && islandCount == 0)
            {
                vrHandler.mode = 0;
                vrHandler.enemy = 0;
                vrHandler.initVR();
            }
            else if (islandCount > 0 && monsterCount == 0) {
                vrHandler.mode = 1;
                vrHandler.enemy = 0;
                vrHandler.initVR();
            }
        }

        if (mTransitionManager != null) {
    		mTransitionManager.Play(goingBackToAR);
		}

       
            
        #endif
    }

	public IEnumerator MoveAlongPath(List<Tile> tiles) {
		Debug.Log("Moving along "+tiles.Count+" tiles");
		while(tiles.Count > 0) { //Move Player from Tile to Tile
			this.isAnimationPlaying = true;
			Debug.Log("Moving to tile "+tiles[0].gameObject.name);
			this.playerTile.RemovePlayerFromTile();
			tiles[0].SetPlayerOnTile();
			tiles.RemoveAt(0);
			yield return new WaitUntil(() => this.isAnimationPlaying == false );
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
		this.isPlayerMovable = true;
	}

	private IEnumerator AnimateRotation(Transform startTransform, int degrees, Vector3 newPosition, Tile newTile) {
		if(degrees>180 ) {
			degrees = -(360-degrees);
		} else if(degrees <-180) {
			degrees += 360;
		}
		float startdegrees = startTransform.localRotation.eulerAngles.y;
		float duration = 1f;
		float time = 0;
		while( time<duration && degrees != 0) {//Rotate
			time += Time.deltaTime;
			Vector3 newRot = startTransform.localRotation.eulerAngles;
			newRot.y = startdegrees + degrees*this.rotateAnimation.Evaluate(time/duration);
			playerFigure.transform.localRotation = Quaternion.Euler(newRot);
			yield return new WaitForEndOfFrame();
		}

		
		// Move Player to new Position
		List<Tile> oldFarNeighbours = this.oldPlayerTile.GetFarNeighbourTiles();
		List<Tile> newFarNeighbours = newTile.GetFarNeighbourTiles();
		foreach(Tile tile in oldFarNeighbours) {
			if(newFarNeighbours.Contains(tile) == false) {
				tile.HideTile();
			}
		}
		foreach(Tile tile in newFarNeighbours) {
			if(oldFarNeighbours.Contains(tile) == false) {
				tile.ShowTile();
			}
		}
		playerFigure.transform.localPosition = newPosition;
		playerFigure.GetComponent<Animator>().Play("MoveForward");
		CheckNewPosition(newTile);

		yield return new WaitForEndOfFrame();
		Invoke("OnAnimationEnded",1.1f);
	}

	private void CheckNewPosition(Tile tile) { //Check if something special happens on the new Tile
		EnvironmentType newEnvironment = tile.Environment.type;
		if(newEnvironment == EnvironmentType.Banana) {
			Player.Instance.addHealth(20);
			tile.RemoveChildObjects(); //Remove Banana
			tile.Environment.ApplySettings(EnvironmentManager.Instance.GetEnvironmentByType(EnvironmentType.Ocean));
		}
	}

	private void OnAnimationEnded() {
		this.isAnimationPlaying = false;
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
