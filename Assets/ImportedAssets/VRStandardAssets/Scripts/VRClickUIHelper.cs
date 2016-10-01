using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using VRStandardAssets.Utils;


public class VRClickUIHelper : MonoBehaviour {

	public UnityEvent onClickEvents;

	public void InvokeEvents() {
		 onClickEvents.Invoke();
	}

	void OnEnable() {
		VRInteractiveItem item = gameObject.GetComponent<VRInteractiveItem>();
		item.OnClick += InvokeEvents;
	}

	void OnDisable() {
		VRInteractiveItem item = gameObject.GetComponent<VRInteractiveItem>();
		item.OnClick -= InvokeEvents;
	}
}
