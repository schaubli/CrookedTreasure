using UnityEngine;
using System.Collections;

public class GameDesignHelper : MonoBehaviour {

	
	public void DrawCard() {
		CardManager.Instance.DrawCard();
	}

	public void PrintCardStack(){
		CardManager.Instance.PrintCardStack();
	}

	public void Explore() {
		//EnvironmentManager.Instance.ExploreEnvironments();
	}
}
