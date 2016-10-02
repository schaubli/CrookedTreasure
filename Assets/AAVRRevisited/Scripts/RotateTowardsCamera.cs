using UnityEngine;
using System.Collections;

public class RotateTowardsCamera : MonoBehaviour {

	private Transform cameraTrans;
	// Use this for initialization
	void Start () {
		this.cameraTrans = ((Camera) FindObjectOfType(typeof(Camera))).gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
	this.transform.rotation = cameraTrans.rotation;
	}
}
