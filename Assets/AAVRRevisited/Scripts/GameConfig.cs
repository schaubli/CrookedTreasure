using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour {


	[Range(0.0f, 1.0f)]
	public float monsterProbability;
	public float minDistanceBetweenTreasures;
	public float minTreasureDistance;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private static GameConfig instance = null;
	public static GameConfig Instance
	{
		get
		{
			if (instance == null)
				instance = (GameConfig)FindObjectOfType(typeof(GameConfig));
			return instance;
		}
	}
}
