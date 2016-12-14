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
	public EntityMover moverOnTile;

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

	public int X {
		get{ return positionVector.X; }
	}

	public int Y {
		get{ return positionVector.Y; }
	}

	public TileVec GetPositionVector() {
		return this.positionVector;
	}

	#region Neighbour_Tiles

	private Tile left;
	public Tile Left{
		get{
			if(this.left == null) {
				this.left = TileManager.Instance.GetTile(new TileVec(X-1,Y));
			}
			return this.left;
		}
	}

	private Tile right;
	public Tile Right{
		get{
			if(this.right == null) {
				this.right = TileManager.Instance.GetTile(new TileVec(X+1,Y));
			}
			return this.right;
		}
	}

	private Tile topLeft;
	public Tile TopLeft{
		get{
			if(this.topLeft == null) {
				this.topLeft = TileManager.Instance.GetTile(new TileVec((Y%2==0?X-1:X),Y+1));
			}
			return this.topLeft;
		}
	}

	private Tile topRight;
	public Tile TopRight{
		get{
			if(this.topRight == null) {
				this.topRight = TileManager.Instance.GetTile(new TileVec((Y%2==0?X:X+1),Y+1));
			}
			return this.topRight;
		}
	}

	private Tile bottomLeft;
	public Tile BottomLeft{
		get{
			if(this.bottomLeft == null) {
				this.bottomLeft = TileManager.Instance.GetTile(new TileVec((Y%2==0?X-1:X),Y-1));
			}
			return this.bottomLeft;
		}
	}

	private Tile bottomRight;
	public Tile BottomRight{
		get{
			if(this.bottomRight == null) {
				this.bottomRight = TileManager.Instance.GetTile(new TileVec((Y%2==0?X:X+1),Y-1));
			}
			return this.bottomRight;
		}
	}

	public Tile GetNeighbourInDirection(TileDirection direction) {
		switch(direction) {
			case TileDirection.Left:
				return this.Left;
			case TileDirection.Right:
				return this.Right;
			case TileDirection.TopLeft:
				return this.TopLeft;
			case TileDirection.TopRight:
				return this.TopRight;
			case TileDirection.BottomLeft:
				return this.BottomLeft;
			case TileDirection.BottomRight:
				return this.BottomRight;
			default:
			return this;
		}
	}

	#endregion

	public Environment Environment {
		get {
			return this.environment;
		} 
	}

	public void SetPlayerOnTile() {
		/*Vector3 newPos = transform.position;
		newPos.y += 0.001f;
		transform.position = newPos;*/
	}

	public void RemovePlayerFromTile() {
		/*Vector3 newPos = transform.position;
		//newPos.y -= 0.001f;
		transform.position = newPos;*/
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
		if(PlayerController.Instance.IsPlayerMovable == true) {
			PlayerController.Instance.IsPlayerMovable = false;
			Path pathToTile = PathFinder.FindPath(PlayerController.Instance.currentTile, this, PathParameter.WalkableAndVisible);
			if(pathToTile != null) {
				StartCoroutine(PlayerController.Instance.MoveAlongPath(this));
			} else {
				PlayerController.Instance.IsPlayerMovable = true;
				Debug.Log("Could not find path to "+gameObject.name);
			}
		} else {
			Debug.Log("Tried to move but Ship is still in animation");
			//Debug.Log(PlayerController.Instance.isRotationAnimationPlaying);
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
		int ownX = this.X;
		int ownY = this.Y;
		int otherX = tile.X;
		int otherY = tile.Y;
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
			if( neighbour != null && neighbour.Environment.IsWalkable == true && neighbour.moverOnTile == null) {
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
			if( neighbour != null && neighbour.Environment.IsWalkable == true && neighbour.IsDiscovered == true && neighbour.moverOnTile == null) {
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
		int ownX = this.X;
		int ownY = this.Y;
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
		int ownX = this.X;
		int ownY = this.Y;
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

	public void DeactivateGameObject() {
		gameObject.SetActive(false);
	}
}
