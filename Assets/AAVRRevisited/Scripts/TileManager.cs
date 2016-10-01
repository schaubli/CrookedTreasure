using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

	public GameObject tilePrefab;
	public Material playerTileMaterial;
	public Material defaultTileMaterial;
	public GameObject playerPrefab;
	private GameObject playerFigure;
	private Tile playerTile;
	int width = 10;
	int height = 10;
	public Dictionary<TileVec, Tile> tiles = new Dictionary<TileVec, Tile>();
	private static TileManager instance;

	// Use this for initialization
	void Start () {
		instance = this;
		if(tilePrefab == null) {
			Debug.LogError("Tile prefab is not defined in TileManager");
		}
		for(int y = -height; y<height; y++) {
			for(int x = -width; x<width; x++) {
				AddNewTile(x,y);
			}
		}
		SortHierarchy();
		Tile rootTile = GetTile(new TileVec(0,0));
		rootTile.SetPlayerOnTile();
		this.playerTile = rootTile;
		rootTile.ShowTile();
		rootTile.ShowNeighbours();
	}

	private void SortHierarchy() {
		List<Tile> list = new List<Tile>();
     	//Fill with objects of CustomClass...
		foreach(Tile tile in tiles.Values) {
			list.Add(tile);
		}
		list.Sort((x,y) => (int) ((x.DistanceFromRoot()*10000+x.AngleFromRoot()).CompareTo(y.DistanceFromRoot()*10000+y.AngleFromRoot())));
		foreach(Tile tile in list) {
			tile.transform.SetAsLastSibling();
		}
	}

	public Tile AddNewTile(int x, int y) {
		GameObject tileGameObject = (GameObject) Instantiate(tilePrefab, Tile.GetWorldPosition(x,y), Quaternion.identity);
		Tile newTile = tileGameObject.GetComponent<Tile>();
		newTile.gameObject.transform.SetParent(this.transform);
		newTile.InitTile(x,y);
		tiles.Add(newTile.GetPositionVector(), newTile);
		newTile.gameObject.name = "Tile ("+newTile.GetX()+", "+newTile.GetY()+")";
		
		return newTile;
	}

	public Tile GetTile(TileVec vec) {
		Tile t;
		if(tiles.TryGetValue(vec, out t)){
			return t;
		}
		//Debug.LogWarning("Could not find Tile at position "+vec.ToString()+" "+t);
		return null;
	}

    public static TileManager Instance { 
		get {
			if(instance == null)
			{
				GameObject obj = new GameObject();
				instance = (TileManager) obj.AddComponent( typeof(TileManager));
			}
    		return instance;
    	}
	}

	public void SetPlayerTile(Tile tile) {
		Tile oldPlayerTile = this.playerTile;
		if(oldPlayerTile == null){
			oldPlayerTile = tile;
		}
		this.playerTile = tile;
		if(playerFigure == null) {
			playerFigure = (GameObject) Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		}
		playerFigure.transform.localPosition = tile.transform.localPosition; // Move Player to new Position
		//Rotate toward new tile
		Quaternion rotation =  Quaternion.FromToRotation(Vector3.forward, tile.transform.localPosition-oldPlayerTile.transform.localPosition);
		Vector3 eulers = rotation.eulerAngles;
		eulers.z = 0; //Avoid flipping
		rotation = Quaternion.Euler(eulers);
		playerFigure.transform.rotation = rotation;
		playerFigure.GetComponent<Animator>().Play("MoveForward");
	}
}
