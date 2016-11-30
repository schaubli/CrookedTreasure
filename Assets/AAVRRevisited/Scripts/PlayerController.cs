using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : EntityMover {

	public GameObject playerPrefab;
    private Tile rootTile;
    public int playerStartHealth;
	public int playerMaxHealth;

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
		movingGameObject = (GameObject) Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		movingGameObject.transform.SetParent(this.transform);
		movingGameObject.transform.SetAsFirstSibling();
		lastTile = rootTile;
		this.currentTile = rootTile;
        this.rootTile = rootTile;

		#if ! UNITY_EDITOR_OSX
		mTransitionManager = FindObjectOfType<TransitionManager>();
		if(vrHandlerGameobject != null) {
       		vrHandler = vrHandlerGameobject.GetComponent<VRhandler>();
		}
        #endif
    }

    public override void OnAfterRotation(Vector3 newPosition, Tile newTile) {
		// Move Player to new Position
		List<Tile> oldFarNeighbours = this.lastTile.GetFarNeighbourTiles();
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
		movingGameObject.transform.localPosition = newPosition;
		movingGameObject.GetComponent<Animator>().Play("MoveForward");
		CheckNewPosition(newTile);

		Invoke("OnAnimationEnded",1.1f);
	}

    public override void CheckNewPosition(Tile tile) { //Check if something special happens on the new Tile
    	EntityMover[] movers = FindObjectsOfType(typeof(EntityMover)) as EntityMover[];  
    	foreach(EntityMover mover in movers) {
    		if ( mover.gameObject != this.gameObject) {
    			mover.MoveNext();
    		}
    	}
		EnvironmentType newEnvironment = tile.Environment.type;
		if(newEnvironment == EnvironmentType.Banana) {
			Player.Instance.addHealth(20);
			tile.RemoveChildObjects(); //Remove Banana
			tile.Environment.ApplySettings(EnvironmentManager.Instance.GetEnvironmentByType(EnvironmentType.Ocean));
		}
	}
	
	public override void MovePlayerToTile(Tile tile) {
		lastTile = this.currentTile;
		currentTile = tile;
		if(movingGameObject==null) {
			Debug.LogError("Make Sure there is not another Player active in the scene");
		}
		
		//Rotate toward new tile
		Quaternion rotation =  Quaternion.FromToRotation(Vector3.forward, tile.transform.localPosition-lastTile.transform.localPosition);
		animationCoroutine = AnimateRotation(movingGameObject.transform, Mathf.RoundToInt(rotation.eulerAngles.y-movingGameObject.transform.localRotation.eulerAngles.y), tile.transform.localPosition, tile);
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
                //vrHandler.mode = 1;
                //vrHandler.enemy = 0;
                // Debugging:
                vrHandler.mode = 0;
                vrHandler.enemy = 0;
                vrHandler.initVR();
            }
        }

        if (mTransitionManager != null) {
    		mTransitionManager.Play(goingBackToAR);
		}

       
            
        #endif
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
