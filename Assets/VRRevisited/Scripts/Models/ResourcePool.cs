using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ResourceType {
	Food,
	Wood,
	Stone,
	Rope
}

[System.Serializable]
public class ResourcePool {

	[SerializeField]
	public Resource[] resources = new Resource[4] {	new Resource(ResourceType.Food), 
													new Resource(ResourceType.Wood), 
													new Resource(ResourceType.Stone),
													new Resource(ResourceType.Rope)   };

	public Resource Resource(ResourceType type){ // returns the Resource of the given resource type
		switch(type) {
			case ResourceType.Food:
				return resources[0];
			case ResourceType.Wood:
				return resources[1];
			case ResourceType.Stone:
				return resources[2];
			case ResourceType.Rope:
				return resources[3];
			default:
				Debug.LogError("Could not find Resource for type "+type);
				return new Resource();
		}
	}
}

