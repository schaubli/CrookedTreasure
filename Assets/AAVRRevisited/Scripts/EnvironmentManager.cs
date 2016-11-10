using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pathfind;

public enum RetryCause {
	Overdraw,
	TreasureDistance,
	TreasureRootDistance,
	TreasureReachability,
	RootTile,
	MinRadius
}

//Every Tile will have an Environment and the EnvironmentManager will determine which Tile becomes which environment
public class EnvironmentManager : MonoBehaviour {

	public static EnvironmentManager Instance;
	public bool debugWorldGeneration = false;

	[SerializeField]
	public List<EnvironmentSetting> settings = new List<EnvironmentSetting>();
	private List<Environment> environments = new List<Environment>();
	private Random seedRandom;
	private Dictionary<RetryCause, int> retries = new Dictionary<RetryCause, int>();
	
	public void AssignEnvironments(List<Tile> tiles){
		this.environments.Clear();
		this.retries.Clear();

		//StartCoroutine(DebugCoroutine());

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
		StartCoroutine(GenerateWorld());
	}

	private IEnumerator GenerateWorld() {

		foreach(EnvironmentSetting setting in this.settings) { //Go through all settings
			if(setting.type == EnvironmentType.Ocean) {
				continue;//Skip Ocean, we already set every tile to ocean
			}
			for(int i = 0; i<setting.spawnAmount; i++) { //Set as many Tiles to that setting as set in spawnAmount
				Tile nextTile = TileManager.Instance.RootTile; // The tile we are trying to set the environment to
				bool isPositionOkay = false;
				while(isPositionOkay == false) { //Get a new Random tile until one position is allowed
					if(debugWorldGeneration) {
						yield return new WaitForEndOfFrame();
					}
					int xPos = RandomInt(-TileManager.Instance.Width, TileManager.Instance.Width);
					int yPos = RandomInt(-TileManager.Instance.Height, TileManager.Instance.Height);
					nextTile = TileManager.Instance.GetTile(new TileVec(xPos,yPos));
					isPositionOkay = IsPositionOK(setting, nextTile, i, setting.spawnAmount);//Check if the new Position is valid

					/*if(isPositionOkay == false) {
						Debug.Log("Trying to set "+setting.type+" to tile "+nextTile.gameObject.name);
						/*foreach(RetryCause cause in System.Enum.GetValues(typeof(RetryCause))) {
							if(this.retries.ContainsKey(cause)) {
								Debug.Log("Retried " + this.retries[cause] + " time"+(this.retries[cause]>1?"s":"") +" because of "+cause.ToString());
							}
						}
					}*/

				}
				nextTile.Environment.ApplySettings(setting); //POsition is valid: Set tile to the environment
			}
			yield return new WaitForEndOfFrame();
		}
		DebugWorldGeneration();
		Debug.Log("Finished world generation");
	}



	private bool IsPositionOK(EnvironmentSetting setting, Tile tile, int currentCount, int maxCount) {

		if(tile.Environment.type != EnvironmentType.Ocean) { //Dont allow overwriting an existing environment
			AddRetry(RetryCause.Overdraw);
			return false;
		}
		if(tile == TileManager.Instance.RootTile) { //Dont Place anything on StartTile
			AddRetry(RetryCause.RootTile);
			return false;
		}

		if(tile.transform.localPosition.magnitude < setting.minRadius) {
			AddRetry(RetryCause.MinRadius);
			return false;
		}

		switch(setting.type) {

			case EnvironmentType.Treasure:

				//Dont place treasures close to the center
				if(tile.transform.localPosition.magnitude < GameConfig.Instance.minTreasureDistance) {
					AddRetry(RetryCause.TreasureRootDistance);
					return false;
				}

				//Dont place treasures close to another
				if(currentCount>0) { //There has been placed a treasure before
					Tile otherTreasure = TileManager.Instance.GetTileByEnvironmentType(setting.type);//First found Treasure in hierarchy
					float distance = (otherTreasure.transform.localPosition - tile.transform.localPosition).magnitude;

					if(distance < GameConfig.Instance.minDistanceBetweenTreasures) {
						AddRetry(RetryCause.TreasureDistance);
						return false;
					}
				}

				//Dont place Treasures unreachable for Player
				/*if(PathFinder.IsReachable(tile, TileManager.Instance.RootTile)==false){
					AddRetry(RetryCause.TreasureReachability);
					return false;
				}*/
				if(PathFinder.IsReachable(tile, TileManager.Instance.RootTile) == false) {  //Check if Treasure is Reachable

					//Clear all tiles on the way from treasure to root if treasure is not reachable
					Path forcedPath = PathFinder.FindPath(tile, TileManager.Instance.RootTile, PathParameter.AnyTile);
					List<Tile> tilesOnForcedPath = forcedPath.TilesOnPath();
					foreach(Tile pathTile in tilesOnForcedPath) { 
						if(pathTile.Environment.IsWalkable == false) { // Clear Environment if it is in the way and can not be walked
							//Debug.LogWarning("Cleared tile "+pathTile.gameObject.name +" from "+pathTile.Environment.type);
							pathTile.Environment.ApplySettings(GetEnvironmentByType(EnvironmentType.Ocean));
						}
					}
				}

			break;
			
			default:
			break;
		}
		return true;
	}

	private static int RandomInt(int min, int max) {
		return Random.Range(min, max+1); //max of Random.Range is exclusive so we add 1 to include it
	}

	private void AddRetry(RetryCause cause) {
		if(debugWorldGeneration) {
			Debug.Log("Failed because of "+cause.ToString());
		}
		if(this.retries.ContainsKey(cause)) {
			this.retries[cause] = this.retries[cause] +1;
		} else {
			this.retries.Add(cause, 1);
		}
	}

	private void DebugWorldGeneration() {
		foreach(RetryCause cause in System.Enum.GetValues(typeof(RetryCause))) {
			if(this.retries.ContainsKey(cause)) {
				Debug.Log("Retried " + this.retries[cause] + " time"+(this.retries[cause]>1?"s":"") +" because of "+cause.ToString());
			}
		}
	}


	void OnValidate() {
		int sum = 0;
		foreach(EnvironmentSetting env in settings) {
			if(env.type != EnvironmentType.Ocean){
				sum += env.spawnAmount;
			}
		}
		GetEnvironmentByType(EnvironmentType.Ocean).spawnAmount = ((2*TileManager.Instance.Width+1)*(2*TileManager.Instance.Height+1)-(TileManager.Instance.Width+TileManager.Instance.Height)*4) -1-sum;
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
