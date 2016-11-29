using UnityEngine;
using System.Collections;

public enum TileDirection {
	Left,
	Right,
	TopLeft,
	TopRight,
	BottomLeft,
	BottomRight
}


public class MovingEnemy : MonoBehaviour {


	public Tile currentTile; 
	public TileDirection[] movements;
	private int currentMovementIndex = 0;

	public void MoveNext() {
		int nextIndex = this.currentMovementIndex+1;
		if(nextIndex >=movements.Length) {
			nextIndex = 0;
		}
		
		Tile newTile = this.currentTile.GetNeighbourInDirection(movements[nextIndex]);
		this.currentTile = newTile;
		
		//MoveToTile(newTile);
	}
}
