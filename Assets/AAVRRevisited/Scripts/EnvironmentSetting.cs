using UnityEngine;
using System.Collections;

//We use this class to apply settings for Environments in the Incpector of the EnvironmentManager

[System.Serializable]
public class EnvironmentSetting{


	[SerializeField]
	public EnvironmentType type;
	[SerializeField]
	[HideInInspector]
	public string name =  "Environment";
	[SerializeField]
	[Range(0, 400)] public int spawnAmount;
	[SerializeField]
	public GameObject modelPrefab;
	[SerializeField]
	public bool isWalkable;
	public float minRadius;

	[HideInInspector]
	public EnvironmentType lasttype;
	void OnValidate() {
		if(type != lasttype){
			this.name = type.ToString();
			lasttype = type;
		}
	}
}
