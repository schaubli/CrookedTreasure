using UnityEngine;
using System.Collections;

public class TreasureIsland : MonoBehaviour{

	public Tile tile;
	public int index; //at what position this should be found
	public GameObject icon; //The rotating icon above the treasure island

	void Start() {
		
	}

	public void OnClickedIcon() {
		if(tile.gameObject.activeInHierarchy == true) {
			this.tile.TrySetPlayer();
		}
	}

	public void SetActive() {
		icon.gameObject.SetActive(true);
	}

	public void Visit() {
		icon.gameObject.SetActive(false);
		Debug.Log("Set "+icon.gameObject+" unactive");
	}
}
