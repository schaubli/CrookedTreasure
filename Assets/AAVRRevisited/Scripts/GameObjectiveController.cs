using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameObjectiveController : MonoBehaviour {

	public List<TreasureIsland> treasureIslands;
	public TreasureIsland nextTreasure;

	// Use this for initialization
	void OnEnable () {
		instance = this;
		this.treasureIslands = FindObjectsOfType<TreasureIsland>().ToList();
		treasureIslands.Sort((x,y) => (int) ((x.index).CompareTo(y.index)));
		foreach(TreasureIsland island in this.treasureIslands) {
			island.icon.SetActive(false);
		}
		GoToNextState();
	}

	public void GoToNextState() {
		if(treasureIslands.Count>0){
			this.nextTreasure = treasureIslands[0];
			this.nextTreasure.icon.SetActive(true);
			treasureIslands.RemoveAt(0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private static GameObjectiveController instance;
	public static GameObjectiveController Instance { 
		get {
			if(instance == null)
			{
				GameObject obj = new GameObject();
				instance = (GameObjectiveController) obj.AddComponent( typeof(GameObjectiveController));
			}
    		return instance;
    	}
	}
}
