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
		movingGameObject = (GameObject) Instantiate(playerPrefab, rootTile.gameObject.transform.position, Quaternion.identity);
		movingGameObject.transform.SetParent(this.transform);
		movingGameObject.transform.SetAsFirstSibling();
        movingGameObject.transform.localScale = new Vector3(1, 1, 1);
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

    public override IEnumerator OnAfterRotation(Vector3 newPosition, Tile newTile) {
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
		newTile.ShowTile();
		EntityMover[] movers = FindObjectsOfType(typeof(EntityMover)) as EntityMover[];  
    	foreach(EntityMover mover in movers) {
    		if ( mover.gameObject != this.gameObject) {
    			mover.MoveNext();
    		}
    	}

		TileManager.Instance.EnteredNewTile();

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
		int enemyShipCount = 0;
		foreach(Tile t in neighbours) {
			if(t.Environment.type == EnvironmentType.TreasureIsland){
				Debug.Log("Next to Treasure Island");
				if(t.Environment.model.GetComponent<TreasureIsland>() == GameObjectiveController.Instance.nextTreasure) {
					Debug.Log("Starting VR for Treasure Island");
					GameObjectiveController.Instance.GoToNextState();
					islandcount = 3-GameObjectiveController.Instance.treasureIslands.Count;
				}
			}
			if(t.moverOnTile != null || t.Environment.type == EnvironmentType.EnemyShip) {
				Debug.Log("Mover "+t.moverOnTile.gameObject.name);
				TileManager.Instance.moversInScene.Remove(t.moverOnTile);
				Destroy(t.moverOnTile.gameObject, 1.5f);
				t.Environment.type = EnvironmentType.Ocean;
				enemyShipCount++;
			}
		}
		if(islandcount==0 && enemyShipCount == 0) {
			if(Random.value <= GameConfig.Instance.monsterProbability) {
				Debug.Log("Encountered Monster");
				Tile neighbourTile = newTile.GetOceanNeighbour();
				GameObject ARKraken = (GameObject) Instantiate(EnvironmentManager.Instance.GetEnvironmentByType(EnvironmentType.Monster).modelPrefab, neighbourTile.gameObject.transform.position, 
									Quaternion.LookRotation(newTile.transform.localPosition-neighbourTile.transform.localPosition, newTile.transform.up));
				ARKraken.transform.SetParent(TileManager.Instance.transform);
				Destroy(ARKraken, 1.5f);
				monstercount = 1;
			}
		}
        islandcount = 1;
        if (islandcount>0 || monstercount>0 || enemyShipCount>0) {
            if (newTile != this.rootTile)
            {
				Debug.Log("Going to VR Mode");
				yield return StartCoroutine(StartVRMode(islandcount, monstercount, enemyShipCount));
            }
		}
		yield return new WaitForEndOfFrame();
	}

	private IEnumerator StartVRMode(int islandCount, int monsterCount, int enemyShipCount) {
        #if !UNITY_EDITOR_OSX

        bool goingBackToAR = false;

        if (vrHandler != null)
        {
            if (monsterCount > 0)
            {
                vrHandler.mode = 1;
                vrHandler.enemy = 0;
                vrHandler.initVR();
            }
            else if (islandCount > 0)
            {
                vrHandler.mode = 3;
                vrHandler.treasure = islandCount;
                vrHandler.initVR();
            }
            else if (enemyShipCount > 0) {
                vrHandler.mode = 1;
                vrHandler.enemy = 1;
                vrHandler.initVR();
            }
        }

        if (mTransitionManager != null) {
    		mTransitionManager.Play(goingBackToAR);
			Debug.Log("Transition state in AR "+ mTransitionManager.InAR);
			yield return new WaitForSeconds(1);
			while (mTransitionManager.InAR == false){
				yield return new WaitForEndOfFrame();
			}
			Debug.Log("Came back to AR mode");
			TileManager.Instance.ResetPositionToRoot(this.currentTile.transform.localPosition);
		}
		#else
			TileManager.Instance.ResetPositionToRoot(this.currentTile.transform.localPosition);
			yield return new WaitForEndOfFrame();
        #endif
    }
    public void EndVRMode() {
        #if !UNITY_EDITOR_OSX
        if (mTransitionManager != null)
        {

            mTransitionManager.Play(true);
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
