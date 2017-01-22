using UnityEngine;
using System.Collections;

public class CompassNeedleController : MonoBehaviour {
	private Vector3 up;
	
	void Start() {
	}
	void Update () {
		Vector3 vecTowardsTreasure = GameObjectiveController.Instance.nextTreasure.transform.position - PlayerController.Instance.movingGameObject.transform.position;
		Quaternion lastFrameRotation = transform.localRotation;
		Vector3 up = transform.parent.up;
 		vecTowardsTreasure.y = transform.position.y;
 		Quaternion qTo = Quaternion.LookRotation(vecTowardsTreasure, up);  
		
		transform.rotation = qTo;
		
 		transform.localRotation = Quaternion.Slerp(lastFrameRotation, Quaternion.Euler(0,transform.localRotation.eulerAngles.y, 0),0.25f);
	}
}
