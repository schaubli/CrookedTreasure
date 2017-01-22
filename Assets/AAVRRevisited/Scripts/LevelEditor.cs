using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LevelEditor : MonoBehaviour {

	public GameObject reefPrefab;
	// Use this for initialization
	[Button]
	public void ReplaceReefs () {
		foreach(Tile t in  FindObjectsOfType<Tile>()){
			if(t.gameObject.GetComponent<Environment>().type == EnvironmentType.Reef ) {
				DestroyImmediate(t.transform.GetChild(0).gameObject);
				/*GameObject newReef = (GameObject) Instantiate ( reefPrefab, t.gameObject.transform.position, Quaternion.identity);
				newReef.transform.localPosition = new Vector3(newReef.transform.localPosition.x, newReef.transform.localPosition.y, newReef.transform.localPosition.z);
				newReef.transform.RotateAroundLocal(Vector3.up, Random.value*360);
				newReef.transform.SetParent(t.gameObject.transform);*/
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
