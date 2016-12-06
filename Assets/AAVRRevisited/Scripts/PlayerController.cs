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
		EntityMover[] movers = FindObjectsOfType(typeof(EntityMover)) as EntityMover[];  
    	foreach(EntityMover mover in movers) {
    		if ( mover.gameObject != this.gameObject) {
    			mover.MoveNext();
    		}
    	}

		//Check for Enemies and Islands
		
		EnvironmentType newEnvironment = newTile.Environment.type;
		if(newEnvironment == EnvironmentType.Banana) {
			Player.Instance.addHealth(20);
			newTile.RemoveChildObjects(); //Remove Banana
			newTile.Environment.ApplySettings(EnvironmentManager.Instance.GetEnvironmentByType(EnvironmentType.Ocean));
		}

		List<Tile> neighbours = newTile.GetNeighbourTiles();
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
            if (newTile != this.rootTile)
            {
                StartVRMode(islandcount, monstercount);
            }
		}
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
