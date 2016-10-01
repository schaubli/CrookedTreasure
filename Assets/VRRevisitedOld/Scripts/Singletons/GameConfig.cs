using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPerformable {
	void Perform();
}

public class GameConfig : MonoBehaviour {

	public static GameConfig Instance;
	[SerializeField]
	public List<Card> cards; // All Card in the Game
	[Range(0,1)]
	public float improveRate = 0.5f;
	[Range(0,5)]
	public int improvableEnvironments = 0;



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
