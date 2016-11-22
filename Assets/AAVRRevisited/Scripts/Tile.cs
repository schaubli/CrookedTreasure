using UnityEngine;
using Pathfind;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	//This is not the position of the Tile in Unity coordinates but in the Tile coordinate System
	public TileVec positionVector;
	private bool isPlayerOnTile = false;
	private bool isShown = false;
	private bool isDiscovered = false;
	//private bool isRimPiece = false;
	private MeshRenderer tileRenderer;
	private Environment environment;
	public GameObject macroEnvironment;

	public void InitTile(int x, int y) {
		InitTile(new TileVec(x,y));
	}

	public void InitTile(TileVec vec) {
		this.positionVector = vec;
		this.gameObject.transform.localRotation = Quaternion.Euler(0,90,0);
		this.tileRenderer = gameObject.GetComponent<MeshRenderer>();
		if(tileRenderer == null) Debug.LogWarning("No MeshRenderer found for Tile "+gameObject.name);
		this.environment = gameObject.GetComponent<Environment>();
		if(environment == null) Debug.LogWarning("No Environment found for Tile "+gameObject.name);
	}

	public int GetX() {
		return positionVector.X;
	}

	public int GetY() {
		return positionVector.Y;
	}

	public Environment Environment {
		get {
			return this.environment;
		} 
	}

	public void SetPlayerOnTile() {
		this.isPlayerOnTile = true;
		tileRenderer.material = TileManager.Instance.playerTileMaterial;
		/*Vector3 newPos = transform.position;
		newPos.y += 0.001f;
		transform.position = newPos;*/
		PlayerController.Instance.MovePlayerToTile(this);
	}

	public void RemovePlayerFromTile() {
		this.isPlayerOnTile = false;
		tileRenderer.material = TileManager.Instance.defaultTileMaterial;
		/*Vector3 newPos = transform.position;
		//newPos.y -= 0.001f;
		transform.position = newPos;*/
	}

	public TileVec GetPositionVector() {
		return this.positionVector;
	}


	public static Vector3 GetWorldPosition(float x, float y) {
		if(Mathf.Abs(y%2)==1 ) { //Shift right for uneven row number
			x+=0.5f;
		}
		float z = y * 0.8660254038f; //sqrt(1-0.5*0.5)
		y = 0; //Set Y to 0
		return new Vector3(x,y,z);
	}
	
	public void TrySetPlayer() {
		if(PlayerController.Instance.isPlayerMovable == true) {
			PlayerController.Instance.isPlayerMovable = false;
			Path pathToTile = PathFinder.FindPath(PlayerController.Instance.playerTile, this, PathParameter.WalkableAndVisible);
			if(pathToTile != null) {
				StartCoroutine(PlayerController.Instance.MoveAlongPath(pathToTile.TilesOnPath()));
			} else {
				PlayerController.Instance.isPlayerMovable = true;
				Debug.Log("Could not find path to "+gameObject.name);
			}
		} else {
			Debug.Log("Player is not movable");
		}
	}

	/*
	public static void MovePlayerToTile(Tile oldTile, Tile newTile) {
		oldTile.RemovePlayerFromTile();
		newTile.SetPlayerOnTile();
	}*/

	public void ShowNeighbours() {
		List<Tile> neighbours = this.GetFarNeighbourTiles();
		foreach(Tile neighbour in neighbours) {
			if( neighbour != null) {
				if(neighbour.isShown == false) {
					neighbour.ShowTile();
				}
			}
		}
	}
	public void HideNeighbours() {
		List<Tile> neighbours = this.GetFarNeighbourTiles();
		foreach(Tile neighbour in neighbours) {
			if( neighbour != null) {
				if(neighbour.isShown == true) {
					neighbour.HideTile();
				}
			}
		}
	}

	public void ShowFarNeighbours() {
		List<Tile> neighbours = this.GetFarNeighbourTiles();
		foreach(Tile neighbour in neighbours) {
			if( neighbour != null) {
				if(neighbour.isShown == false) {
					neighbour.ShowTile();
				}
			}
		}
	}
	public void HideFarNeighbours() {
		List<Tile> neighbours = this.GetFarNeighbourTiles();
		foreach(Tile neighbour in neighbours) {
			if( neighbour != null) {
				if(neighbour.isShown == true) {
					neighbour.HideTile();
				}
			}
		}
	}

	public bool IsNeighbour(Tile tile) {
		int ownX = this.GetX();
		int ownY = this.GetY();
		int otherX = tile.GetX();
		int otherY = tile.GetY();
		if(Mathf.Abs(ownY%2) == 1) {
			if(ownY == otherY+1 || ownY == otherY-1) {
				if(otherX == ownX || otherX == ownX+1) {
					return true;
				}
			} else if(ownY == otherY) {
				if(otherX ==ownX-1 || otherX == ownX+1) {
					return true;
				}
			}
		} else {
			if(ownY == otherY+1 || ownY == otherY-1) {
				if(otherX == ownX || otherX == ownX-1) {
					return true;
				}
			} else if(ownY == otherY) {
				if(otherX >=ownX-1 && otherX <= ownX+1) {
					return true;
				}
			}
		}
		return false;
	}

	public List<Tile> GetWalkableNeighbours() {
		List<Tile> tiles = new List<Tile>();
		foreach(TileVec tilevec in GetNeighbours()) {
			Tile neighbour = TileManager.Instance.GetTile(tilevec);
			if( neighbour != null && neighbour.Environment.IsWalkable == true) {
				tiles.Add(neighbour);
			}/* else {
				if(neighbour == null) {
					Debug.Log("No neighbour found at position "+tilevec);
				} else {
					Debug.Log("Neighbour: "+neighbour.gameObject.name+" is not walkable");
				}
			}*/
		}
		return tiles;
	}

	public List<Tile> GetWalkableAndVisibleNeighbours() {
		List<Tile> tiles = new List<Tile>();
		foreach(TileVec tilevec in GetNeighbours()) {
			Tile neighbour = TileManager.Instance.GetTile(tilevec);
			if( neighbour != null && neighbour.Environment.IsWalkable == true && neighbour.IsDiscovered == true) {
				tiles.Add(neighbour);
			} /*else {
				if(neighbour == null) {
					Debug.Log("No neighbour found at position "+tilevec);
				} else if(neighbour.Environment.IsWalkable == false) {
					Debug.Log("Neighbour: "+neighbour.gameObject.name+" is not walkable");
				} else {
					Debug.Log("Neighbour: "+neighbour.gameObject.name+" is not discovered");
				}
			}*/
		}
		return tiles;
	}

	public List<Tile> GetNeighbourTiles() {
		List<Tile> tiles = new List<Tile>();
		foreach(TileVec tilevec in GetNeighbours()) {
			Tile neighbour = TileManager.Instance.GetTile(tilevec);
			if( neighbour != null) {
				tiles.Add(neighbour);
			}
		}
		return tiles;
	}

	public List<Tile> GetFarNeighbourTiles() {
		List<Tile> tiles = new List<Tile>();
		foreach(TileVec tilevec in GetFarNeighbours()) {
			Tile neighbour = TileManager.Instance.GetTile(tilevec);
			if( neighbour != null) {
				tiles.Add(neighbour);
			}
		}
		return tiles;
	}
	
	public List<TileVec> GetNeighbours() {
		List<TileVec> neighbours = new List<TileVec>();
		int ownX = this.GetX();
		int ownY = this.GetY();
		if(Mathf.Abs(ownY%2) == 1) {
			neighbours.Add(new TileVec(ownX, ownY+1));
			neighbours.Add(new TileVec(ownX+1, ownY+1));
			neighbours.Add(new TileVec(ownX, ownY-1));
			neighbours.Add(new TileVec(ownX+1, ownY-1));
		} else {
			neighbours.Add(new TileVec(ownX-1, ownY+1));
			neighbours.Add(new TileVec(ownX, ownY+1));
			neighbours.Add(new TileVec(ownX-1, ownY-1));
			neighbours.Add(new TileVec(ownX, ownY-1));
		}
		neighbours.Add(new TileVec(ownX-1, ownY));
		neighbours.Add(new TileVec(ownX+1, ownY));
		return neighbours;
	}

	public List<TileVec> GetFarNeighbours() {
		List<TileVec> neighbours = new List<TileVec>();
		int ownX = this.GetX();
		int ownY = this.GetY();
		if(Mathf.Abs(ownY%2) == 1) {
			neighbours.Add(new TileVec(ownX, ownY+1));
			neighbours.Add(new TileVec(ownX+1, ownY+1));
			neighbours.Add(new TileVec(ownX-1, ownY+1));
			neighbours.Add(new TileVec(ownX+2, ownY+1));

			neighbours.Add(new TileVec(ownX, ownY-1));
			neighbours.Add(new TileVec(ownX+1, ownY-1));
			neighbours.Add(new TileVec(ownX-1, ownY-1));
			neighbours.Add(new TileVec(ownX+2, ownY-1));
		} else {
			neighbours.Add(new TileVec(ownX-1, ownY+1));
			neighbours.Add(new TileVec(ownX, ownY+1));
			neighbours.Add(new TileVec(ownX-2, ownY+1));
			neighbours.Add(new TileVec(ownX+1, ownY+1));

			neighbours.Add(new TileVec(ownX-1, ownY-1));
			neighbours.Add(new TileVec(ownX, ownY-1));
			neighbours.Add(new TileVec(ownX-2, ownY-1));
			neighbours.Add(new TileVec(ownX+1, ownY-1));
		}
		neighbours.Add(new TileVec(ownX-1, ownY));
		neighbours.Add(new TileVec(ownX+1, ownY));
		neighbours.Add(new TileVec(ownX-2, ownY));
		neighbours.Add(new TileVec(ownX+2, ownY));

		neighbours.Add(new TileVec(ownX-1, ownY-2));
		neighbours.Add(new TileVec(ownX, ownY-2));
		neighbours.Add(new TileVec(ownX+1, ownY-2));

		neighbours.Add(new TileVec(ownX-1, ownY+2));
		neighbours.Add(new TileVec(ownX, ownY+2));
		neighbours.Add(new TileVec(ownX+1, ownY+2));

		return neighbours;
	}

	public float DistanceFromRoot() {
		return (transform.localPosition.magnitude);
	}
	public float AngleFromRoot() {
		float angle = Vector3.Angle(new Vector3(1,0,0), transform.localPosition);
		if(transform.localPosition.z<0){
			angle = 360-angle;
		}
		return angle;
	}

	public void RemoveChildObjects() {
		Transform[] childs = this.gameObject.GetComponentsInChildren<Transform>();
		foreach(Transform trans in childs) {
			if(trans != this.transform) {
				Destroy(trans.gameObject);
			}
		}
	}

	public bool IsShown{
		get{
			return this.isShown;
		}
	}
	public bool IsDiscovered {
		get {
			return this.isDiscovered;
		}
	}

	public void ShowTile() {
		//tileRenderer.enabled = true;
		gameObject.SetActive(true);
		this.gameObject.GetComponent<Animator>().Play("FadeIn");
		this.isShown = true;
		this.isDiscovered = true;
		if(macroEnvironment != null) {
			macroEnvironment.SetActive(true);
		}
	}
	
	public void HideTile() {
		//tileRenderer.enabled = false;
		//gameObject.SetActive(false);
		this.gameObject.GetComponent<Animator>().Play("FadeOut");
		this.isShown = false;
	}

}
