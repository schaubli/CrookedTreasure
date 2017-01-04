using UnityEngine;
using System.Collections;

public class TileDepending : MonoBehaviour { //Controlls the visibility of renderes that should be hidden with tiles and vuforia


	public Tile tile; //The tile of gameObject
	void Start() {
		this.tile = GetComponentInParent<Tile>();
		if(tile == null) {
			Debug.LogWarning("No Tile found for Tile depending GameObject "+gameObject.name);
		}
	}

	void OnEnable() {
		TileManager.OnEnterNewTile += UpdateVisibility;
	}

	void OnDisable() {
		TileManager.OnEnterNewTile -= UpdateVisibility;
	}

	public void UpdateVisibility() {
		if(this.tile!=null) {
			Renderer[] myRenderers = this.gameObject.GetComponentsInChildren<Renderer>(true);
			if(tile.IsShown == false ) {
				foreach (Renderer component in myRenderers)
				{
					component.enabled = false;
				}
			} else {
				foreach (Renderer component in myRenderers)
				{
					component.enabled = true;
				}
			}
		}
	}
}
