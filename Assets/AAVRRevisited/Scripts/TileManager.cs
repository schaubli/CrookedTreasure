using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

	public GameObject tilePrefab;
	public Material playerTileMaterial;
	public Material defaultTileMaterial;
	public int width = 10;
	public int height = 10;
	public Dictionary<TileVec, Tile> tiles = new Dictionary<TileVec, Tile>();
	public List<Tile> tilelist= new List<Tile>();
	private static TileManager instance;
	private Tile rootTile;
	public Tile RootTile{get {return this.rootTile;}}

	// Use this for initialization
	void Start () {
		if(tilePrefab == null) {
			Debug.LogError("Tile prefab is not defined in TileManager");
		}
		for(int y = -height; y<=height; y++) {
			for(int x = -width; x<=width; x++) {
				AddNewTile(x,y);
			}
		}
		Application.targetFrameRate = 60;
		SortHierarchy();
		this.rootTile = GetTile(new TileVec(0,0));
		PlayerController playerControll = (PlayerController) (FindObjectOfType(typeof(PlayerController)));
		EnvironmentManager.Instance.AssignEnvironments(this.tilelist);
		playerControll.Initiate(rootTile);
		this.rootTile.SetPlayerOnTile();
		this.rootTile.ShowTile();
		this.rootTile.ShowNeighbours();
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

	public Tile AddNewTile(int x, int y) {
		GameObject tileGameObject = (GameObject) Instantiate(tilePrefab, Vector3.zero,Quaternion.identity);
		Tile newTile = tileGameObject.GetComponent<Tile>();
		newTile.gameObject.transform.SetParent(this.transform);
		newTile.transform.localPosition = Tile.GetWorldPosition(x,y);
        newTile.InitTile(x,y);
		tiles.Add(newTile.GetPositionVector(), newTile);
		tilelist.Add(newTile);
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
