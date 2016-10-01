using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnvironmentOldType
{
	Ocean,
	Island,
	Monster,
	Coast, 
	Forest,
	Mountain,
	Valley
}

[System.Serializable]
public class EnvironmentOld : MonoBehaviour{
	[SerializeField]
	new public string name =  "Environment";
	[SerializeField]
	public EnvironmentOldType type;
	[SerializeField]
	[HideInInspector]
	private ResourcePool pool;
	private bool isImproved = false;

	[Header("Drop values")]
	[SerializeField]
	[Range (0f, 1f)]
	private float foodDropRate = 0.5f;
	[SerializeField]
	[Range (0f, 10f)]
	private int foodDropAmount = 1;

	[Space(10)]
	[SerializeField]
	[Range (0f, 1f)]
	private float woodDropRate = 0.5f;
	[SerializeField]
	[Range (0f, 10f)]
	private int woodDropAmount = 1;

	[Space(10)]
	[SerializeField]
	[Range (0f, 1f)]
	private float stoneDropRate = 0.5f;
	[SerializeField]
	[Range (0f, 10f)]
	private int stoneDropAmount = 1;

	[Space(10)]
	[SerializeField]
	[Range (0f, 1f)]
	private float ropeDropRate = 0.5f;
	[SerializeField]
	[Range (0f, 10f)]
	private int ropeDropAmount = 1;

	[Header("Improved drop values")]
	[SerializeField]
	[Range (0f, 1f)]
	private float foodImprovedDropRate = 0.5f;
	[SerializeField]
	[Range (0f, 10f)]
	private int foodImprovedDropAmount = 1;

	[Space(10)]
	[SerializeField]
	[Range (0f, 1f)]
	private float woodImprovedDropRate = 0.5f;
	[SerializeField]
	[Range (0f, 10f)]
	private int woodImprovedDropAmount = 1;

	[Space(10)]
	[SerializeField]
	[Range (0f, 1f)]
	private float stoneImprovedDropRate = 0.5f;
	[SerializeField]
	[Range (0f, 10f)]
	private int stoneImprovedDropAmount = 1;

	[Space(10)]
	[SerializeField]
	[Range (0f, 1f)]
	private float ropeImprovedDropRate = 0.5f;
	[SerializeField]
	[Range (0f, 10f)]
	private int ropeImprovedDropAmount = 1;

	[Space(10)]
	[SerializeField]
	[TextArea(3,10)]
	public string explorationDescription;


	public Dictionary<ResourceType, float> dropRates;
	public Dictionary<ResourceType, int> dropAmounts;
	public Dictionary<ResourceType, float> improvedDropRates;
	public Dictionary<ResourceType, int> improvedDropAmounts;

	void Start() {
		InitDictionaries();
	}

	public int GetDrop(ResourceType type) {
		int drops = 0;
		float rate;
		int amount;
		if(isImproved == false) {
			rate = dropRates[type];
			amount = dropAmounts[type];
		} else {
			rate = improvedDropRates[type];
			amount = improvedDropAmounts[type];
		}
		for(int i = 0; i<amount; i++) {
			if(Random.value<=rate) {
				drops++;
			}
		}
		Debug.Log("Environment "+ this.name +" dropped "+drops+ " "+type.ToString());
		return drops;
	}

	public bool IsImprovable() {
		if(type == EnvironmentOldType.Island) {
			return false;
		}
		return !isImproved;
	}

	public EnvironmentOld(EnvironmentOldType type) {
		this.type = type;
		this.name = type.ToString();
	}

	public ResourcePool GetResourcePool() {
		if(pool == null) {
			pool = gameObject.GetComponent<ResourcePool>();
			if(pool==null){
				pool = gameObject.AddComponent<ResourcePool>() as ResourcePool;
			}
		}
		return pool;
	}

	public void SetImproved() {
		this.isImproved = true;
		Debug.Log(explorationDescription);
	}

	private EnvironmentOldType lasttype;
	void OnValidate() {
		if(type != lasttype){
			this.name = type.ToString();
			lasttype = type;
		}
	}

	private void InitDictionaries(){
		dropRates = new Dictionary<ResourceType, float>();
		dropRates.Add(ResourceType.Food, foodDropRate);
		dropRates.Add(ResourceType.Wood, woodDropRate);
		dropRates.Add(ResourceType.Stone, stoneDropRate);
		dropRates.Add(ResourceType.Rope, ropeDropRate);

		dropAmounts = new Dictionary<ResourceType, int>();
		dropAmounts.Add(ResourceType.Food, foodDropAmount);
		dropAmounts.Add(ResourceType.Wood, woodDropAmount);
		dropAmounts.Add(ResourceType.Stone, stoneDropAmount);
		dropAmounts.Add(ResourceType.Rope, ropeDropAmount);

		improvedDropRates = new Dictionary<ResourceType, float>();
		improvedDropRates.Add(ResourceType.Food, foodImprovedDropRate);
		improvedDropRates.Add(ResourceType.Wood, woodImprovedDropRate);
		improvedDropRates.Add(ResourceType.Stone, stoneImprovedDropRate);
		improvedDropRates.Add(ResourceType.Rope, ropeImprovedDropRate);

		improvedDropAmounts = new Dictionary<ResourceType, int>();
		improvedDropAmounts.Add(ResourceType.Food, foodImprovedDropAmount);
		improvedDropAmounts.Add(ResourceType.Wood, woodImprovedDropAmount);
		improvedDropAmounts.Add(ResourceType.Stone, stoneImprovedDropAmount);
		improvedDropAmounts.Add(ResourceType.Rope, ropeImprovedDropAmount);
	}
}
