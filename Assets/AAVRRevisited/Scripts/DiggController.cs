using UnityEngine;
using System.Collections.Generic;
using System.Linq;

	public enum TreasureChestMode {
		Compass, 
		CrookedTreasure
	}
public class DiggController : MonoBehaviour {


	public int maxDiggCount = 3; //Max amount that each diggable should be digged

	public float diggOffset = 0.2f; //Distance that that floor should be moved down with each dig;
	public GameObject treasureChestPrefab;
	public GameObject compassPrefab;
	public GameObject currentTreasure;
	public List<Diggable> diggables;
	// Use this for initialization
	void OnEnable () {
		this.diggables = GetComponentsInChildren<Diggable>().ToList();
		this.Reset(TreasureChestMode.Compass);
	}

	public void Reset(TreasureChestMode mode) {
		foreach(Diggable diggable in this.diggables) {
			diggable.Reset();
		}
		if(this.currentTreasure != null) {
			DestroyImmediate(this.currentTreasure);
		}
		PlaceTreasure(mode);
	}

	private void PlaceTreasure(TreasureChestMode mode) {
		if(mode == TreasureChestMode.CrookedTreasure) {
			if(this.treasureChestPrefab == null) {
				Debug.LogError("No Treasure Prefab defined in DiggController");
				return;
			}
			Diggable randomDiggable = diggables[Random.Range(0,this.diggables.Count)]; // Select a random diggable where to place the treasure
			Quaternion treasureRotation = Quaternion.LookRotation(this.transform.position-randomDiggable.transform.position, Vector3.up); //Rotate so that it always looks towards the center
			Vector3 treasurePosition = randomDiggable.transform.position - new Vector3(0,(this.maxDiggCount-2)*this.diggOffset,0);
			this.currentTreasure = (GameObject) Instantiate(this.treasureChestPrefab, treasurePosition, treasureRotation);
			this.currentTreasure.transform.SetParent(this.transform);
			this.currentTreasure.transform.SetAsFirstSibling();
		} else {
			if(this.compassPrefab == null) {
				Debug.LogError("No Compass Prefab defined in DiggController");
				return;
			}
			Diggable randomDiggable = diggables[Random.Range(0,this.diggables.Count)]; // Select a random diggable where to place the treasure
			Quaternion treasureRotation = Quaternion.LookRotation(this.transform.position-randomDiggable.transform.position, Vector3.up); //Rotate so that it always looks towards the center
			Vector3 treasurePosition = randomDiggable.transform.position - new Vector3(0,(this.maxDiggCount-2)*this.diggOffset,0);
			this.currentTreasure = (GameObject) Instantiate(this.compassPrefab, treasurePosition, treasureRotation);
			this.currentTreasure.transform.SetParent(this.transform);
			this.currentTreasure.transform.SetAsFirstSibling();
		}
		
	}

	private static DiggController instance;
	public static DiggController Instance {
		get {
			if(instance==null) {
				instance = (DiggController) FindObjectOfType<DiggController>();
				if(instance == null) {
					Debug.LogWarning("Could not find instance of DiggController");
				}
			}
			return instance;
 		}
	}
}
