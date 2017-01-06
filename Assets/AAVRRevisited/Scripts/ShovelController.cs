using UnityEngine;
using System.Collections;

public class ShovelController : MonoBehaviour {


	public float shovelInDistance = 0.2f;
	public Transform shovelTransform;
	public Transform mainCamTransform;
	// Use this for initialization
	void Start () {
		this.mainCamTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Quaternion cameraRotation = mainCamTransform.rotation;
		float x = cameraRotation.eulerAngles.x;
		if(x > 90) {
			x = 0;
		}
		if(x<0) {
			x=0;
		}
		cameraRotation = Quaternion.Euler(x, cameraRotation.eulerAngles.y, cameraRotation.eulerAngles.z);
		this.transform.rotation = cameraRotation;
	}

	public void OnClick() {
		GetComponentInChildren<Animator>().Play("ShovelUp");
	}

	public void OnHoverOverDiggable(Diggable dig) {
		shovelTransform.Translate(new Vector3(0,-shovelInDistance,0));
	}
	public void OnEndHoverOverDiggable(Diggable dig) {
		shovelTransform.Translate(new Vector3(0,shovelInDistance,0));
	}

	private static ShovelController instance;
	public static ShovelController Instance {
		get {
			if(instance==null) {
				instance = (ShovelController) FindObjectOfType<ShovelController>();
				if(instance == null) {
					Debug.LogWarning("Could not find instance of ShovelController");
				}
			}
			return instance;
 		}
	}
}
