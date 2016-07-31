using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour {

	public static EnvironmentManager Instance;
	[SerializeField]
	public List<Environment> environments = new List<Environment>();


	public ResourcePool GetResourcePool(ResourceActionTarget target) {
		switch(target) {
			case ResourceActionTarget.Player:
				return Player.Instance.GetResourcePool();
			case ResourceActionTarget.Island:
				return this.GetResourcePool(EnvironmentType.Island);
			case ResourceActionTarget.Mountain:
				return this.GetResourcePool(EnvironmentType.Mountain);
			case ResourceActionTarget.Ocean:
				return this.GetResourcePool(EnvironmentType.Ocean);
			case ResourceActionTarget.Coast:
				return this.GetResourcePool(EnvironmentType.Coast);
			case ResourceActionTarget.Forest:
				return this.GetResourcePool(EnvironmentType.Forest);
			case ResourceActionTarget.Valley:
				return this.GetResourcePool(EnvironmentType.Valley);
			default:
				return null;
		}
	}

	public ResourcePool GetResourcePool(EnvironmentType type) {
		foreach(Environment environment in environments) {
			if(environment.type == type){
				return environment.GetResourcePool();
			}
		}
		Debug.LogError("Could not find environment for type "+type);
		return null;
	}



	void Awake()
	{
		//Check if instance already exists
		if (Instance == null)
		    
		    //if not, set instance to this
		    Instance = this;

		//If instance already exists and it's not this:
		else if (Instance != this)
		    
		    //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
		    Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}
}
