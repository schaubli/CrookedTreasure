using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnvironmentType{
	Ocean,
	Island
}

[System.Serializable]
public class Environment : MonoBehaviour{
	[SerializeField]
	new public string name =  "Environment";
	[SerializeField]
	public EnvironmentType type;
	private GameObject model;

	public void ApplySettings(EnvironmentSetting setting) {
		DeleteSettings();
		if(setting.modelPrefab != null) {
			this.model = (GameObject) Instantiate(setting.modelPrefab, transform.position, GetRandomQuat());
			model.transform.SetParent(this.transform);
		}
		this.type = setting.type;
		this.name = setting.name;
	}

	public void DeleteSettings() {
		if(this.model!=null) {
			Destroy(this.model);
		}
	}

	public static Quaternion GetRandomQuat() {
		int rand = (int) (Random.value * 6f);
		Quaternion quat = Quaternion.identity;
		Quaternion sixtydegrees = Quaternion.AngleAxis(60,Vector3.up);
		for(int i = 0; i<rand; i++) {
			quat *= sixtydegrees;
		}
		return quat;
	}

	public Environment(EnvironmentType type) {
		this.type = type;
		this.name = type.ToString();
	}

	[HideInInspector]
	public  EnvironmentType lasttype;

}
