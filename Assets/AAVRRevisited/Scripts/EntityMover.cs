using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfind;

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
	private bool isPlayerMovable = true;
	private bool isMovingAnimationPlaying = false;
	public Tile currentTile; 
	[HideInInspector]
	public Tile lastTile;
	public AnimationCurve rotateAnimation;
	public IEnumerator animationCoroutine;
	public TileDirection[] movements;
	private int currentMovementIndex = 0;

	private static bool debugMovement = false;

	public bool IsPlayerMovable {
		get{ 
			return this.isPlayerMovable;
		}
		set {
			this.isPlayerMovable = value;
		}
	}

	public IEnumerator MoveAlongPath(Tile targetTile) { //Called when clicking on
		Path currentPathToTile = PathFinder.FindPath(PlayerController.Instance.currentTile,targetTile, PathParameter.WalkableAndVisible);
		Debug.Log("Moving along "+currentPathToTile.TilesOnPath().Count+" tiles");
		while(currentPathToTile != null && PlayerController.Instance.currentTile != targetTile && currentPathToTile.TilesOnPath().Count>0) { //Move Player until we reached target or cant move there
			this.isMovingAnimationPlaying = true;
			Debug.Log("Moving to tile "+currentPathToTile.TilesOnPath()[0].gameObject.name);
			yield return StartCoroutine(MovePlayerToTile(currentPathToTile.TilesOnPath()[0]));
			currentPathToTile = PathFinder.FindPath(currentPathToTile.TilesOnPath()[0], targetTile, PathParameter.WalkableAndVisible);
			DebugMovement("Waiting until end of animation");
			yield return new WaitForEndOfFrame();
			while (isMovingAnimationPlaying == true) {
				DebugMovement("Coroutine waiting for animation to end on "+gameObject.name);
				yield return new WaitForEndOfFrame();
			}
		}
		this.IsPlayerMovable = true;
		DebugMovement("Moving along path finished");
	}

	public IEnumerator AnimateRotation(Transform startTransform, int degrees, Vector3 newPosition, Tile newTile) { //Called before moving to tile
		if(degrees>180 ) {
			degrees = -(360-degrees);
		} else if(degrees <-180) {
			degrees += 360;
		}
		float startdegrees = startTransform.localRotation.eulerAngles.y;
		float duration = 1f/180f*(Mathf.Abs(degrees)+90);
		float time = 0;
		DebugMovement("Rotating towards new tile");
		while( time<duration && degrees != 0) {//Rotate
			time += Time.deltaTime;
			Vector3 newRot = startTransform.localRotation.eulerAngles;
			newRot.y = startdegrees + degrees*this.rotateAnimation.Evaluate(time/duration);
			movingGameObject.transform.localRotation = Quaternion.Euler(newRot);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
		DebugMovement("Rotation finished");
		movingGameObject.transform.localPosition = newPosition;
		DebugMovement("Moving forward on");
		movingGameObject.GetComponent<Animator>().Play("MoveForward");
		OnAfterRotation(newPosition, newTile);
	}

	public virtual void OnAfterRotation(Vector3 newPosition, Tile newTile) {

	}

	public IEnumerator MovePlayerToTile(Tile tile) { // Called by tile
		lastTile = this.currentTile;
		currentTile = tile;
		lastTile.moverOnTile = null;
		if(movingGameObject==null) {
			Debug.LogError("Make sure there is not another Player active in the scene");
		}
		yield return new WaitForEndOfFrame();
		//Rotate toward new tile
		Quaternion rotation =  Quaternion.FromToRotation(Vector3.forward, tile.transform.localPosition-lastTile.transform.localPosition);
		animationCoroutine = AnimateRotation(movingGameObject.transform, Mathf.RoundToInt(rotation.eulerAngles.y-movingGameObject.transform.localRotation.eulerAngles.y), tile.transform.localPosition, tile);
		yield return StartCoroutine(animationCoroutine);
		currentTile.moverOnTile = this;
	}

	public void OnAnimationEnded() { // Called at end of MoveForward animation
		this.isMovingAnimationPlaying = false;
		DebugMovement("Moving finished");
	}

	public void MoveNext() { // Called 
		Tile newTile = this.currentTile.GetNeighbourInDirection(movements[currentMovementIndex]);

		//Debug.Log(gameObject.name+" moving to "+movements[currentMovementIndex]);
		
		this.currentMovementIndex+=1;
		if(currentMovementIndex >=movements.Length) {
			currentMovementIndex = 0;
		}
		
		//MoveToTile(newTile);
		this.isMovingAnimationPlaying = true;
		DebugMovement("Moving "+gameObject.name+" to tile "+newTile.gameObject.name);
		StartCoroutine(MovePlayerToTile(newTile));
	}

	private void DebugMovement(string debugMessage) {
		if(debugMovement) {
			Debug.Log(debugMessage);
		}
	}

	void OnValidate() {
		if(this.currentTile != null) {
			this.transform.position = currentTile.transform.position;
		}
	}
}
