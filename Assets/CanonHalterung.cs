using UnityEngine;
using System.Collections;

public class CanonHalterung : MonoBehaviour {

    public GameObject Player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var playerEulerX = Player.transform.eulerAngles.x;
        //if (playerEulerX < 0)
        //{
            Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, playerEulerX + 20);
            transform.rotation = Quaternion.Euler(eulerRotation);
        //}
    }
}
