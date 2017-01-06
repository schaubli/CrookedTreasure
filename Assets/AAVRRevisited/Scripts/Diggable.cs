using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class Diggable : MonoBehaviour {

	public int diggCount = 0; //How often this has been digged
	public bool isFinished = false;
	public Vector3 startPosition;

	public void Digg() {
		ShovelController.Instance.OnClick();
		if(this.isFinished == false && this.diggCount < DiggController.Instance.maxDiggCount) {
			this.diggCount++;
			Vector3 surfacePosition = this.transform.position;
			surfacePosition.y -= DiggController.Instance.diggOffset;
			this.transform.position = surfacePosition;

			if(this.diggCount == DiggController.Instance.maxDiggCount) {
				this.isFinished = true;
			}
		}
	}

	public void OnOver() { // When Player hovers over Collider
		ShovelController.Instance.OnHoverOverDiggable(this);
	}

	public void OnOut() { // When Player stops hovering over Collider
		ShovelController.Instance.OnEndHoverOverDiggable(this);
	}
	void OnEnable() {
		this.startPosition = this.transform.localPosition;
		VRInteractiveItem item = gameObject.GetComponent<VRInteractiveItem>();
		item.OnOver += OnOver;
		item.OnClick += Digg;
		item.OnOut += OnOut;
	}

	void OnDisable() {
		VRInteractiveItem item = gameObject.GetComponent<VRInteractiveItem>();
		item.OnOver -= OnOver;
		item.OnClick -= Digg;
		item.OnOut -= OnOut;
	}

	public void Reset() {
		if(this.startPosition == Vector3.zero) {
			OnEnable();
		}
		this.isFinished = false;
		this.diggCount = 0;
		this.transform.localPosition = startPosition;
	}
}
