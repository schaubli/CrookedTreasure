using UnityEngine;
using System.Collections;

public class SeedGenerator : MonoBehaviour {

	public int seed;
	void Awake () {
		if(seed != 0 ) {
			Random.seed = this.seed;
		}
		Debug.Log("Generated world with seed "+Random.seed);
	}

	public float GetNewRand() {
		return Random.value;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
