using UnityEngine;
using System.Collections;

[System.Serializable]
public class Resource {


	[SerializeField]
	[HideInInspector]
	public string name;
	[SerializeField]
	public readonly ResourceType type;


	[SerializeField]
	public int _quantity;
	[SerializeField]
	public int max;

	public int quantity {
		get{
			return _quantity;
		}
		set{
			_quantity = value;
			if(_quantity<0) {
				Debug.LogWarning("Resource "+this.name+" lower than 0 ("+_quantity+")");
				_quantity = 0;
			} else 
			if(max > 0 && _quantity>max) {
				Debug.LogWarning("Resource "+this.name+" higher than max ("+_quantity+")");
				_quantity = max;
			}
		}
	}

	//Constructors
	public Resource(ResourceType type){
		this.type = type;
		this.name = type.ToString();
	}
	public Resource() :this(ResourceType.Food) {

	}
}
