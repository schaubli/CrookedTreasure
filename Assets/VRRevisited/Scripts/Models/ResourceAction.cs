using UnityEngine;
using System.Collections;

public enum ResourceActionType{
	Add,
	Subtract,
	Multiply,
	Divide
}

public enum ResourceActionTarget{
	Player,
	Island,
	Mountain,
	Ocean,
	Coast,
	Forest,
	Valley
}

[System.Serializable]
public class ResourceAction : IPerformable {
	
	[SerializeField]
	public ResourceActionTarget target;
	[SerializeField]
	public ResourceActionType type;
	[SerializeField]
	public ResourceType resourceType;
	[SerializeField]
	public int amount;

	public void Perform() {
		switch (target){
			case ResourceActionTarget.Player:
				ResourcePool pool = Player.Instance.GetResourcePool();
				switch (type) {
					case ResourceActionType.Add:
						pool.Resource(this.resourceType).quantity += this.amount;
						break;
					case ResourceActionType.Subtract:
						pool.Resource(this.resourceType).quantity -= this.amount;
						break;
					case ResourceActionType.Multiply:
						pool.Resource(this.resourceType).quantity *= this.amount;
						break;
					case ResourceActionType.Divide:
						pool.Resource(this.resourceType).quantity /= this.amount;
						break;
					default:
						break;
				} 
				break;
			default:
			break;
		}
	}
}
