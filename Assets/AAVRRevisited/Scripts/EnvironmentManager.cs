using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


//Every Tile will have an Environment and the EnvironmentManager will determine which Tile becomes which environment
public class EnvironmentManager : MonoBehaviour {

	public static EnvironmentManager Instance;
	public float minDistanceBetweenTreasures;
	public float minTreasureDistance;
	
	[SerializeField]
	public List<EnvironmentSetting> settings = new List<EnvironmentSetting>();
	private List<Environment> environments = new List<Environment>();
	private Random seedRandom;
	
	public void AssignEnvironments(List<Tile> tiles){
		this.environments.Clear();
		
		foreach(Tile t in tiles) {
			this.environments.Add(t.Environment);
			EnvironmentSetting newSettings = GetEnvironmentByType(EnvironmentType.Ocean);

			//Set the center/root tile to water
			if(t.GetX() == 0 && t.GetY() == 0) {//Root tile
				newSettings = GetEnvironmentByType(EnvironmentType.Ocean);
			}

			//Set the outer Tiles reefs
			if(t.GetX() ==-TileManager.Instance.Width || t.GetX() ==+TileManager.Instance.Width || t.GetY() ==-TileManager.Instance.Height || t.GetY() ==+TileManager.Instance.Height) {
				newSettings = GetEnvironmentByType(EnvironmentType.Reef);
			}
			t.Environment.ApplySettings(newSettings);
		}


		foreach(EnvironmentSetting setting in this.settings) { //Go through all settings
			if(setting.type == EnvironmentType.Ocean) {
				continue;//Skip Ocean, we already set every tile to ocean
			}
			for(int i = 0; i<setting.spawnAmount; i++) { //Set as many Tiles to that setting as set in spawnAmount
				Tile nextTile = TileManager.Instance.RootTile; // The tile we are trying to set the environment to
				bool isPositionOkay = false;
				while(isPositionOkay == false) { //Get a new Random tile until one position is allowed
					int xPos = RandomInt(-TileManager.Instance.Width, TileManager.Instance.Width);
					int yPos = RandomInt(-TileManager.Instance.Width, TileManager.Instance.Width);
					nextTile = TileManager.Instance.GetTile(new TileVec(xPos,yPos));
					isPositionOkay = IsPositionOK(setting.type, nextTile, i, setting.spawnAmount);//Check if the new Position is valid
				}
				nextTile.Environment.ApplySettings(setting); //POsition is valid: Set tile to the environment
			}
		}
	}

	public bool IsPositionOK(EnvironmentType type, Tile tile, int currentCount, int maxCount) {

		if(tile.Environment.type != EnvironmentType.Ocean) { //Dont allow overwriting an existing environment
			return false;
		}
		switch(type) {

			case EnvironmentType.Treasure:
				//Dont place treasures close to the center
				if(tile.transform.localPosition.magnitude<minTreasureDistance) {
					return false;
				}
				//Dont place treasures close to another
				if(currentCount>0) { //There has been placed a treasure before
					Tile otherTreasure = TileManager.Instance.GetTileByEnvironmentType(type);//First found Treasure in hierarchy
					float distance = (otherTreasure.transform.localPosition - tile.transform.localPosition).magnitude;
					if(distance < minDistanceBetweenTreasures) {
						return false;
					}
				}
			break;
			
			default:
			break;
		}
		return true;
	}

	private static int RandomInt(int min, int max) {
		return Random.Range(min, max+1); //max of Random.Range is exclusive so we add 1
	}
	void OnValidate() {
		int sum = 0;
		foreach(EnvironmentSetting env in settings) {
			if(env.type != EnvironmentType.Ocean){
				sum += env.spawnAmount;
			}
		}
		GetEnvironmentByType(EnvironmentType.Ocean).spawnAmount = 400 -sum;
	}
	public EnvironmentSetting GetEnvironmentByType(EnvironmentType type) {
		foreach(EnvironmentSetting setting in this.settings) {
			if(setting.type == type) {
				return setting;
			}
		}
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
