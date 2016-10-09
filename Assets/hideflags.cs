using UnityEngine;
using System.Collections;

public class hideflags : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Mesh mat = gameObject.GetComponent<MeshFilter>().mesh;
		mat.hideFlags = HideFlags.None;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
