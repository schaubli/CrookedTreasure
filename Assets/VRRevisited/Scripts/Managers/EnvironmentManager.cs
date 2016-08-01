using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour {

	public static EnvironmentManager Instance;
	[SerializeField]
	public List<Environment> environments = new List<Environment>();
	private float improveRate;
	private int improvedEnvironments = 0;
	private int improvableEnvironments = 0;


	public void ExploreEnvironments() {
		if(improvedEnvironments<improvableEnvironments){
			if(Random.value <= improveRate) {
				bool improvable = false;
				Environment env = null;
				while(improvable == false) {
					env = environments[Random.Range(0,environments.Count)];
					improvable = env.IsImprovable();
				}
				env.SetImproved();
				improvedEnvironments++;
			} else {
				Debug.Log("Found nothing special");
			}
		} else {
			Debug.Log("You explored the whole island");
		}
	}

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
		return GetEnvironment(type).GetResourcePool();
	}

	public Environment GetEnvironment(EnvironmentType type) {
		foreach(Environment environment in environments) {
			if(environment.type == type){
				return environment;
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

	void Start() {
		Init();
	}

	private void Init() {//get cards that are in game from GameConfig and shuffle them
		environments.Clear();
		environments = gameObject.GetComponentsInChildren<Environment>().ToList();
		improveRate = GameConfig.Instance.improveRate;
		this.improvableEnvironments = GameConfig.Instance.improvableEnvironments;
		if(improvableEnvironments > this.environments.Count) {
			improvableEnvironments = environments.Count-1;//-1 because we cant improve Island
		}
	}
}
