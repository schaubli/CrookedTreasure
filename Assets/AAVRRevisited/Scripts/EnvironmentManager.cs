using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


//Every Tile will have an Environment and the EnvironmentManager will determine which Tile becomes which environment
public class EnvironmentManager : MonoBehaviour {

	public static EnvironmentManager Instance;
	[SerializeField]
	public List<EnvironmentSetting> settings = new List<EnvironmentSetting>();
	private List<Environment> environments = new List<Environment>();
	
	public void AssignEnvironments(List<Tile> tiles){
		this.environments.Clear();
		foreach(Tile t in tiles) {
			this.environments.Add(t.Environment);
			t.Environment.ApplySettings(settings[0]);
		}
	}

	void OnValidate() {
		int sum = 0;
		foreach(EnvironmentSetting env in settings) {
			sum += env.spawnAmount;
		}
		float factor = 400f/sum;
		foreach(EnvironmentSetting env in settings) {
			env.spawnAmount = (int) (env.spawnAmount*factor);
			if(env.type != env.lasttype){
				env.name = env.type.ToString();
				env.lasttype = env.type;
			}
		}
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
