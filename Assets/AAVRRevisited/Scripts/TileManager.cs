using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

	public GameObject tilePrefab;
	public Material playerTileMaterial;
	public Material defaultTileMaterial;
	public bool generateRandomMap;
	public int width = 10;
	public int height = 10;
	public GameObject[] levels; // Array that contains all levels
	public Dictionary<TileVec, Tile> tiles = new Dictionary<TileVec, Tile>();
	public List<Tile> tilelist= new List<Tile>();
	private static TileManager instance;
	private Tile rootTile;
	public Tile RootTile{get {return this.rootTile;}}

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		if(tilePrefab == null) {
			//Debug.LogError("Tile prefab is not defined in TileManager");
			DestroyImmediate(this.gameObject);
			return;
		}

		if(this.generateRandomMap) {
			this.GenerateRandomMap();
			EnvironmentManager.Instance.AssignEnvironments(this.tilelist);
			this.rootTile = GetTile(new TileVec(0,0));
			PlacePlayerOnTile(rootTile);
		} else {
			this.GenerateMapFromPrefab(this.levels[0]);
			PlacePlayerOnTile(rootTile); // Root tile is found in the generating process
		}

		SortHierarchy();

		
	}

	private void GenerateRandomMap() { // Generates a map of the given size and assigns random environments
		for(int y = -height; y<=height; y++) {
			for(int x = -width; x<=width; x++) {
				AddNewTile(x,y);
			}
		}
	}

	private void GenerateMapFromPrefab(GameObject gameObj) {
		GameObject level = Instantiate<GameObject>(gameObj);
		Tile[] newTiles = level.GetComponentsInChildren<Tile>();
		foreach(Tile tile in newTiles) {
			tile.InitTile(TileVec.FromTransform(tile.transform));
			this.AddExistingTile(tile);
			if(tile.gameObject.tag == "Respawn") {
				this.rootTile = tile;
			}
			tile.gameObject.SetActive(false);
			if(tile.macroEnvironment != null) {
				tile.macroEnvironment.SetActive(false);
			}
		}
	}

	private void PlacePlayerOnTile(Tile tile) {
		((PlayerController) (FindObjectOfType(typeof(PlayerController)))).Initiate(rootTile);
		this.rootTile.SetPlayerOnTile();
		this.rootTile.ShowTile();
		this.rootTile.ShowFarNeighbours();
	}
	
	public int TileCount{
		get{
			return 4*(width*height);
		}
	}

	public int Width{get{return this.width;}}
	public int Height{get{return this.height;}}

	private void SortHierarchy() {
		tilelist.Sort((x,y) => (int) ((x.DistanceFromRoot()*10000+x.AngleFromRoot()).CompareTo(y.DistanceFromRoot()*10000+y.AngleFromRoot())));
		int index = 0;
		foreach(Tile tile in tilelist) {
			tile.transform.SetAsLastSibling();
			tile.gameObject.name = "Tile "+index++;
		}
	}

	public Tile AddNewTile(int x, int y) { //Adds a new Tile at the given position;
		GameObject tileGameObject = (GameObject) Instantiate(tilePrefab, Vector3.zero,Quaternion.identity);
		Tile newTile = tileGameObject.GetComponent<Tile>();
		newTile.gameObject.transform.SetParent(this.transform);
		newTile.transform.localPosition = Tile.GetWorldPosition(x,y);
        newTile.InitTile(x,y);
		tiles.Add(newTile.GetPositionVector(), newTile);
		this.tilelist.Add(newTile);
		newTile.gameObject.name = "Tile ("+newTile.X+", "+newTile.Y+")";

        return newTile;
	}

	private void AddExistingTile(Tile tile) { //Adds an existing tile to the tilelist
		this.tilelist.Add(tile);
		this.tiles.Add(tile.GetPositionVector(), tile);
	}

	public Tile GetTile(TileVec vec) {
		Tile t;
		if(tiles.TryGetValue(vec, out t)){
			return t;
		}
		//Debug.LogWarning("Could not find Tile at position "+vec.ToString()+" "+t);
		return null;
	}

	public Tile GetTileByEnvironmentType(EnvironmentType type) {
		foreach(Tile tile in tilelist) {
			if(tile.Environment.type == type) {
				return tile;
			}
 		}
		//Debug.LogWarning("Could not find Tile at position "+vec.ToString()+" "+t);
		return null;
	}

    public static TileManager Instance { 
		get {
			if(instance == null)
			{
				instance = (TileManager)FindObjectOfType(typeof(TileManager));
				if(instance == null) {
					GameObject obj = new GameObject();
					instance = (TileManager) obj.AddComponent( typeof(TileManager));
				}
			}
    		return instance;
    	}
	}

    public void ResetTiles() {
        foreach (Tile tile in tiles.Values)
        {
            Destroy(tile.gameObject);
        }
        Destroy(PlayerController.Instance.playerFigure.gameObject);
        tiles = new Dictionary<TileVec, Tile>();
        tilelist = new List<Tile>();
        this.Start();
    }
}
