using UnityEngine;
using System.Collections;

[System.Serializable]
public class Environment {
	[SerializeField]
	public string name = "Environment";
	[SerializeField]
	public ResourcePool resourcePool = new ResourcePool();

}
