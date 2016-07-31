using UnityEngine;
using System.Collections;

public enum EnvironmentType{
	Island,
	Mountain,
	Ocean,
	Coast,
	Forest,
	Valley
}

[System.Serializable]
public class Environment : MonoBehaviour{
	[SerializeField]
	new public string name =  "Environment";
	[SerializeField]
	public EnvironmentType type;
	[SerializeField]
	private ResourcePool pool;

	public Environment(EnvironmentType type) {
		this.type = type;
		this.name = type.ToString();
	}

	public ResourcePool GetResourcePool() {
		if(pool == null) {
			pool = gameObject.GetComponent<ResourcePool>();
			if(pool==null){
				pool = gameObject.AddComponent<ResourcePool>() as ResourcePool;
			}
		}
		return pool;
	}
}
