using UnityEngine;
using System.Collections;

public enum ResourceActionType{
	Add,
	Subtract,
	Multiply,
	Divide
}

[System.Serializable]
public class ResourceAction : IPerformable {
	[SerializeField]
	public ResourcePool target;
	[SerializeField]
	public ResourceType resourceType;
	[SerializeField]
	public int amount;

	public void Perform() {
		target.Resource(this.resourceType).quantity += this.amount;
	}
}
