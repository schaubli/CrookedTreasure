using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public GameObject playerPrefab;
    [HideInInspector]
    public GameObject playerFigure;
	private Tile playerTile;
	private Tile oldPlayerTile; //Tile the player was last
	public int playerStartHealth;
	public int playerMaxHealth;
	[HideInInspector]
	public bool isPlayerMovable = true;


	// Use this for initialization
	public void Initiate (Tile rootTile) {
		instance = this;
		playerFigure = (GameObject) Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		playerFigure.transform.SetParent(this.transform);
		playerFigure.transform.SetAsFirstSibling();
		oldPlayerTile = rootTile;
		this.playerTile = rootTile;
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
		PlayRotationAnimation(rotation.eulerAngles.y-playerFigure.transform.localRotation.eulerAngles.y);
		playerFigure.transform.localRotation = rotation;
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
			StartVRMode(islandcount, monstercount);
		}
        Player.Instance.removeHealth(10);
	}

	private void StartVRMode(int islandCount, int monsterCount) {
		//Start VR Mode and show the correct amount of islands and monsters

		
	}

	public void PlayRotationAnimation(float angle) {
		switch(Mathf.RoundToInt(angle)) {
			case 60:
			case -300:
				playerFigure.GetComponent<Animator>().Play("RotateRight60");
				playerFigure.GetComponent<Animator>().Play("MoveForward");
				Invoke("EnablePlayerMovement",1.8f);
			break;
			case 120:
			case-240:
				playerFigure.GetComponent<Animator>().Play("RotateRight120");
				playerFigure.GetComponent<Animator>().Play("MoveForward");
				Invoke("EnablePlayerMovement",1.8f);
			break;
			case 180:
			case -180:
				playerFigure.GetComponent<Animator>().Play("RotateRight180");
				playerFigure.GetComponent<Animator>().Play("MoveForward");
				Invoke("EnablePlayerMovement",1.8f);
			break;
			case -120:
			case 240:
				playerFigure.GetComponent<Animator>().Play("RotateLeft120");
				playerFigure.GetComponent<Animator>().Play("MoveForward");
				Invoke("EnablePlayerMovement",1.8f);
			break;
			case -60:
			case 300:
				playerFigure.GetComponent<Animator>().Play("RotateLeft60");
				playerFigure.GetComponent<Animator>().Play("MoveForward");
				Invoke("EnablePlayerMovement",1.8f);
			break;
			default:
				playerFigure.GetComponent<Animator>().Play("MoveForwardShort");
				Invoke("EnablePlayerMovement",1.1f);
			break;
		}
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
