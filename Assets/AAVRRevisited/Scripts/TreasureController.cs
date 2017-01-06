using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class TreasureController : MonoBehaviour {

	// Use this for initialization
	public void Open() {
		GetComponentInChildren<Animator>().Play("TreasureChestOpen");
		Invoke("ActivateParticles", 3);
	}

	private void ActivateParticles() {
		GetComponentInChildren<ParticleSystem>().Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
