using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPerformable {
	void Perform();
}

public class GameConfig : MonoBehaviour {

	public static GameConfig Instance;
	[SerializeField]
	public ResourcePool resourcepool; // All Resources in the Game
	[SerializeField]
	public Environment[] environments = new Environment[5]; // All Environments in the Game
	[SerializeField]
	public List<ResourceAction> actions;


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
		actions[0].Perform();
	}
}
