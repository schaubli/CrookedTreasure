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

    private TriggerType triggerType = TriggerType.VR_TRIGGER;
    private TransitionManager mTransitionManager;
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

        mTransitionManager = FindObjectOfType<TransitionManager>();
    }
	
	public void MovePlayerToTile(Tile tile) {
		oldPlayerTile = this.playerTile;
		playerTile = tile;
		if(playerFigure==null) {
			Debug.LogError("Make Sure there is not another Player active in the scene");
		}
		playerFigure.transform.localPosition = tile.transform.localPosition; // Move Player to new Position
		//Rotate toward new tile
		Quaternion rotation =  Quaternion.FromToRotation(Vector3.forward, tile.transform.localPosition-oldPlayerTile.transform.localPosition);
		playerFigure.transform.localRotation = rotation;
		playerFigure.GetComponent<Animator>().Play("MoveForward");
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
        mTransitionManager.Play(goingBackToAR);

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
