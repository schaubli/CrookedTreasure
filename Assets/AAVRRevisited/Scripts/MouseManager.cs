using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MouseManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*
		if(Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;

			if(Physics.Raycast(ray, out hitInfo)) {
				Tile tile = hitInfo.collider.gameObject.GetComponent<Tile>();
				if(tile.IsShown){
					tile.TrySetPlayer();
				}
				//Debug.Log("Raycast hit Tile at position x=" + tile.GetX()+", y="+tile.GetY());
			} else {
				//Debug.Log("Hit nuthin");
			}
		}
        */

        if (Input.GetKeyDown(KeyCode.Escape)) {
            // TileManager.Instance.ResetTiles();
            SceneManager.LoadScene(0);
        }

    }
}
