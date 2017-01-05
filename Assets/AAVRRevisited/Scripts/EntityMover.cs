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
	public AnimationCurve moveForwardAnimation;
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
			if(time>duration) {
				time=duration;
			}
			Vector3 newRot = startTransform.localRotation.eulerAngles;
			newRot.y = startdegrees + degrees*this.rotateAnimation.Evaluate(time/duration);
			movingGameObject.transform.localRotation = Quaternion.Euler(newRot);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
		DebugMovement("Rotation finished");
		yield return StartCoroutine(this.AnimateForward(newTile.transform.localPosition));
		yield return StartCoroutine(OnAfterRotation(newPosition, newTile));
	}

	public IEnumerator AnimateForward(Vector3 endPosition) { // Should take 1s; Endposition has to be local in the tile coordinate system.
		Vector3 startPosition = this.movingGameObject.transform.localPosition;
		Vector3 movementVector = endPosition-startPosition; //Vector that points from start to end
		float time = 0;
		DebugMovement("Moving towards new Tile");
		while(time <1) {
			time += Time.deltaTime;
			if(time>1) {
				time=1;
			}
			this.movingGameObject.transform.localPosition = startPosition+this.moveForwardAnimation.Evaluate(time)*movementVector;
			yield return new WaitForEndOfFrame();
		}
		OnAnimationEnded();
	}

	public virtual IEnumerator OnAfterRotation(Vector3 newPosition, Tile newTile) {
		yield return new WaitForEndOfFrame();
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

	public virtual void OnAnimationEnded() { // Called at end of MoveForward animation
		this.isMovingAnimationPlaying = false;
		DebugMovement("Moving finished");
	}

	public void MoveNext() { // Called 
		Tile newTile = this.currentTile.GetNeighbourInDirection(movements[currentMovementIndex]);
		if(newTile.IsShown == false ) {
			Renderer[] meshsInMovingObject = this.gameObject.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer component in meshsInMovingObject)
			{
				component.enabled = false;
			}
		} else {
			Renderer[] meshsInMovingObject = this.gameObject.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer component in meshsInMovingObject)
			{
				component.enabled = true;
			}
		}
		
		this.currentTile.Environment.type = EnvironmentType.Ocean;
		newTile.Environment.type = EnvironmentType.EnemyShip;

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
