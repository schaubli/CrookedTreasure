using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Card {

	[SerializeField]
	public string name;
	[SerializeField]
	[TextArea(3,10)]
	public string description;
	[SerializeField]
	public List<ResourceAction> resourceActions;


	public void TriggerEvent() {
		foreach(ResourceAction action in resourceActions){
			action.Perform();
		}
	}
}
