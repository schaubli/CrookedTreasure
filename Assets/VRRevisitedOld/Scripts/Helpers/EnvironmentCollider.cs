using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class EnvironmentCollider : MonoBehaviour, IPointerClickHandler {

	public EnvironmentOldType type;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Text>().text = this.type.ToString();
	}

	public void OnPointerClick(PointerEventData eventData) {
		Debug.Log("Selected Environment "+type.ToString());
		EnvironmentOldManager.Instance.GetEnvironmentOld(type).GetDrop(ResourceType.Food);
		EnvironmentOldManager.Instance.GetEnvironmentOld(type).GetDrop(ResourceType.Wood);
		EnvironmentOldManager.Instance.GetEnvironmentOld(type).GetDrop(ResourceType.Stone);
		EnvironmentOldManager.Instance.GetEnvironmentOld(type).GetDrop(ResourceType.Rope);
	}
}
