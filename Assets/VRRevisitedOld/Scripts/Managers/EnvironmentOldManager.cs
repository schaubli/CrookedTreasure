using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentOldManager : MonoBehaviour {

	public static EnvironmentOldManager Instance;
	[SerializeField]
	public List<EnvironmentOld> EnvironmentOlds = new List<EnvironmentOld>();
	private float improveRate;
	private int improvedEnvironmentOlds = 0;
	private int improvableEnvironmentOlds = 0;


	public void ExploreEnvironmentOlds() {
		if(improvedEnvironmentOlds<improvableEnvironmentOlds){
			if(Random.value <= improveRate) {
				bool improvable = false;
				EnvironmentOld env = null;
				while(improvable == false) {
					env = EnvironmentOlds[Random.Range(0,EnvironmentOlds.Count)];
					improvable = env.IsImprovable();
				}
				env.SetImproved();
				improvedEnvironmentOlds++;
			} else {
				Debug.Log("Found nothing special");
			}
		} else {
			Debug.Log("You explored the whole island");
		}
	}

	public ResourcePool GetResourcePool(ResourceActionTarget target) {
		switch(target) {
		//	case ResourceActionTarget.Player:
		//		return Player.Instance.GetResourcePool();
			case ResourceActionTarget.Island:
				return this.GetResourcePool(EnvironmentOldType.Island);
			case ResourceActionTarget.Mountain:
				return this.GetResourcePool(EnvironmentOldType.Mountain);
			case ResourceActionTarget.Ocean:
				return this.GetResourcePool(EnvironmentOldType.Ocean);
			case ResourceActionTarget.Coast:
				return this.GetResourcePool(EnvironmentOldType.Coast);
			case ResourceActionTarget.Forest:
				return this.GetResourcePool(EnvironmentOldType.Forest);
			case ResourceActionTarget.Valley:
				return this.GetResourcePool(EnvironmentOldType.Valley);
			default:
				return null;
		}
	}

	public ResourcePool GetResourcePool(EnvironmentOldType type) {
		return GetEnvironmentOld(type).GetResourcePool();
	}

	public EnvironmentOld GetEnvironmentOld(EnvironmentOldType type) {
		foreach(EnvironmentOld EnvironmentOld in EnvironmentOlds) {
			if(EnvironmentOld.type == type){
				return EnvironmentOld;
			}
		}
		Debug.LogError("Could not find EnvironmentOld for type "+type);
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
		EnvironmentOlds.Clear();
		EnvironmentOlds = gameObject.GetComponentsInChildren<EnvironmentOld>().ToList();
		improveRate = GameConfig.Instance.improveRate;
		this.improvableEnvironmentOlds = GameConfig.Instance.improvableEnvironments;
		if(improvableEnvironmentOlds > this.EnvironmentOlds.Count) {
			improvableEnvironmentOlds = EnvironmentOlds.Count-1;//-1 because we cant improve Island
		}
	}
}
