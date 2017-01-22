using UnityEngine;
using System.Collections;

public class RotateSelf : MonoBehaviour {

	[Range(0.0f, 10.0f)]
	public float rotationSpeed = 2.0f;
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0,rotationSpeed,0);
	}
}
