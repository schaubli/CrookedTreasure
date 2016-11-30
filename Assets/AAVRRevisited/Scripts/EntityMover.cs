using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TileDirection {
	Left,
	Right,
	TopLeft,
	TopRight,
	BottomLeft,
	BottomRight
}


public class EntityMover : MonoBehaviour {



	public GameObject movingGameObject;
	[HideInInspector]
	public bool isPlayerMovable = true;
	private bool isRotationAnimationPlaying = false;
	public Tile currentTile; 
	[HideInInspector]
	public Tile lastTile;
	public AnimationCurve rotateAnimation;
	public IEnumerator animationCoroutine;
	public TileDirection[] movements;
	private int currentMovementIndex = 0;

	public IEnumerator MoveAlongPath(List<Tile> tiles) {
		Debug.Log("Moving along "+tiles.Count+" tiles");
		while(tiles.Count > 0) { //Move Player from Tile to Tile
			this.isRotationAnimationPlaying = true;
			Debug.Log("Moving to tile "+tiles[0].gameObject.name);
			this.currentTile.RemovePlayerFromTile();
			tiles[0].SetPlayerOnTile();
			tiles.RemoveAt(0);
			yield return new WaitUntil(() => this.isRotationAnimationPlaying == false );
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
		this.isPlayerMovable = true;
	}

	public IEnumerator AnimateRotation(Transform startTransform, int degrees, Vector3 newPosition, Tile newTile) {
		if(degrees>180 ) {
			degrees = -(360-degrees);
		} else if(degrees <-180) {
			degrees += 360;
		}
		float startdegrees = startTransform.localRotation.eulerAngles.y;
		float duration = 1f;
		float time = 0;
		while( time<duration && degrees != 0) {//Rotate
			time += Time.deltaTime;
			Vector3 newRot = startTransform.localRotation.eulerAngles;
			newRot.y = startdegrees + degrees*this.rotateAnimation.Evaluate(time/duration);
			movingGameObject.transform.localRotation = Quaternion.Euler(newRot);
			yield return new WaitForEndOfFrame();
		}
		OnAfterRotation(newPosition, newTile);
	}

	public virtual void OnAfterRotation(Vector3 newPosition, Tile newTile) {
		movingGameObject.transform.localPosition = newPosition;
		movingGameObject.GetComponent<Animator>().Play("MoveForward");
		Invoke("OnAnimationEnded",1.1f);
	}

	public virtual void CheckNewPosition(Tile tile) { //Check if something special happens on the new Tile

	}

	public virtual void MovePlayerToTile(Tile tile) {
		lastTile = this.currentTile;
		currentTile = tile;
		if(movingGameObject==null) {
			Debug.LogError("Make Sure there is not another Player active in the scene");
		}
		
		//Rotate toward new tile
		Quaternion rotation =  Quaternion.FromToRotation(Vector3.forward, tile.transform.localPosition-lastTile.transform.localPosition);
		animationCoroutine = AnimateRotation(movingGameObject.transform, Mathf.RoundToInt(rotation.eulerAngles.y-movingGameObject.transform.localRotation.eulerAngles.y), tile.transform.localPosition, tile);
		StartCoroutine(animationCoroutine);
	}

	private void OnAnimationEnded() {
		this.isRotationAnimationPlaying = false;
	}

	public void MoveNext() {
		Tile newTile = this.currentTile.GetNeighbourInDirection(movements[currentMovementIndex]);
		
		this.currentMovementIndex+=1;
		if(currentMovementIndex >=movements.Length) {
			currentMovementIndex = 0;
		}
		
		//MoveToTile(newTile);

		Debug.Log(gameObject.name+" moved next to "+newTile.gameObject.name);

		if(isRotationAnimationPlaying == false) {
			this.isRotationAnimationPlaying = true;
			Debug.Log("Moving "+gameObject.name+" to tile "+newTile.gameObject.name);
			MovePlayerToTile(newTile);
		} else {
			Debug.Log("Still rotating");
		}
	}
}
