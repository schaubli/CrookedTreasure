using UnityEngine;
using System.Collections;

public class TreasureIsland : MonoBehaviour{

	public Tile tile;
	public int index; //at what position this should be found
	public GameObject icon; //The rotating icon above the treasure island
	public Animator iconAnimator;

	void Start() {
		
	}

	public void OnClickedIcon() {
		if(tile.gameObject.activeInHierarchy == true) {
			this.tile.TrySetPlayer();
		}
	}

	public void Visit() {
		icon.gameObject.SetActive(false);
	}

	void OnValidate() {
		if(this.icon != null && this.iconAnimator == null) {
			this.iconAnimator = icon.GetComponentInChildren<Animator>();
		}
	}
}
