using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerPrefab;
    [HideInInspector]
    public GameObject playerFigure;
	private Tile playerTile;
	private Tile oldPlayerTile; //Tile the player was last
	public int playerStartHealth;
	public int playerMaxHealth;


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
		if(playerFigure.transform==null) {
			Debug.LogError("Make Sure there is not another Player active in the scene");
		}
		playerFigure.transform.localPosition = tile.transform.localPosition; // Move Player to new Position
		//Rotate toward new tile
		Quaternion rotation =  Quaternion.FromToRotation(Vector3.forward, tile.transform.localPosition-oldPlayerTile.transform.localPosition);
		playerFigure.transform.localRotation = rotation;
		playerFigure.GetComponent<Animator>().Play("MoveForward");
        Player.Instance.removeHealth(10);
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
