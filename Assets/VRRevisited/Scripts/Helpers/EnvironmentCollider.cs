using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class EnvironmentCollider : MonoBehaviour, IPointerClickHandler {

	public EnvironmentType type;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Text>().text = this.type.ToString();
	}

	public void OnPointerClick(PointerEventData eventData) {
		Debug.Log("Selected Environment "+type.ToString());
		EnvironmentManager.Instance.GetEnvironment(type).GetDrop(ResourceType.Food);
		EnvironmentManager.Instance.GetEnvironment(type).GetDrop(ResourceType.Wood);
		EnvironmentManager.Instance.GetEnvironment(type).GetDrop(ResourceType.Stone);
		EnvironmentManager.Instance.GetEnvironment(type).GetDrop(ResourceType.Rope);
	}
}
