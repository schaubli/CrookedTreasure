using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	//This is not the position of the Tile in Unity coordinates but in the Tile coordinate System
	private TileVec positionVector;
	private bool isPlayerOnTile = false;
	private bool isShown = false;
	private MeshRenderer tileRenderer;

	public void InitTile(int x, int y) {
		InitTile(new TileVec(x,y));
	}

	public void InitTile(TileVec vec) {
		this.positionVector = vec;
		this.gameObject.transform.rotation = Quaternion.Euler(0,90,0);
		this.tileRenderer = gameObject.GetComponent<MeshRenderer>();
	}

	public int GetX() {
		return positionVector.X;
	}

	public int GetY() {
		return positionVector.Y;
	}

	public void SetPlayerOnTile() {
		this.isPlayerOnTile = true;
		tileRenderer.material = TileManager.Instance.playerTileMaterial;
		Vector3 newPos = transform.position;
		//newPos.y += 0.001f;
		transform.position = newPos;
		TileManager.Instance.SetPlayerTile(this);
		ShowNeighbours();
	}

	public void RemovePlayerFromTile() {
		this.isPlayerOnTile = false;
		tileRenderer.material = TileManager.Instance.defaultTileMaterial;
		Vector3 newPos = transform.position;
		//newPos.y -= 0.001f;
		transform.position = newPos;
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
		List<TileVec> neighbours = this.GetNeighbours();
		foreach(TileVec tilevec in neighbours) {
			Tile neighbour = TileManager.Instance.GetTile(tilevec);
			if( neighbour != null) {
				if(neighbour.isPlayerOnTile == true) {
					neighbour.RemovePlayerFromTile();
					this.SetPlayerOnTile();
					break;
				}
			}
		}
	}

	public void ShowNeighbours() {
		List<TileVec> neighbours = this.GetNeighbours();
		foreach(TileVec tilevec in neighbours) {
			Tile neighbour = TileManager.Instance.GetTile(tilevec);
			if( neighbour != null) {
				if(neighbour.isShown == false) {
					neighbour.ShowTile();
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

	public bool IsShown{
		get{
			return this.isShown;
		}
	}

	public void ShowTile() {
		tileRenderer.enabled = true;
		this.isShown = true;
	}
	
	public void HideTile() {
		tileRenderer.enabled = false;
		this.isShown = false;
	}

}
