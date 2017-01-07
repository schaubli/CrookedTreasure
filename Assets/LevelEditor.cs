using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LevelEditor : MonoBehaviour {

	public GameObject reefPrefab;
	// Use this for initialization
	[Button]
	public void AddReefsAtBorder () {
		foreach(Tile t in  FindObjectsOfType<Tile>()){
			if(t.gameObject.GetComponent<Environment>().type == EnvironmentType.Reef && t.gameObject.transform.childCount == 0) {
				Debug.Log(t.gameObject.name+" found");
				GameObject newReef = (GameObject) Instantiate ( reefPrefab, t.gameObject.transform.position, Quaternion.identity);
				newReef.transform.localPosition = new Vector3(newReef.transform.localPosition.x, 0.2f, newReef.transform.localPosition.z);
				newReef.transform.SetParent(t.gameObject.transform);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
