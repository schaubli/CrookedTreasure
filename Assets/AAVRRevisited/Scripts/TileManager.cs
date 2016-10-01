using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

	public GameObject tilePrefab;
	public Material playerTileMaterial;
	public Material defaultTileMaterial;
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
		PlayerController playerControll = (PlayerController) (FindObjectOfType(typeof(PlayerController)));
		playerControll.Initiate(rootTile);
		rootTile.SetPlayerOnTile();
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
		GameObject tileGameObject = (GameObject) Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
		Tile newTile = tileGameObject.GetComponent<Tile>();
		newTile.gameObject.transform.SetParent(this.transform);
		newTile.transform.localPosition = Tile.GetWorldPosition(x,y);
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
}
