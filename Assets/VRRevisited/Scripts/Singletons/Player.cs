using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public static Player Instance;
	
	private ResourcePool pool;

	public ResourcePool GetResourcePool() {
		if(pool == null) {
			pool = gameObject.GetComponent<ResourcePool>();
			if(pool==null){
				pool = gameObject.AddComponent<ResourcePool>() as ResourcePool;
			}
		}
		return pool;
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
