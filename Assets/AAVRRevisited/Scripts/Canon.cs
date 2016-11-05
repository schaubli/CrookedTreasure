using UnityEngine;
using System.Collections;



public class Canon : MonoBehaviour {

    public GameObject Player;
    private float initRotY;
    // Use this for initialization
    void Start () {
        initRotY = transform.rotation.y;

    }
	
	// Update is called once per frame
	void Update () {
        var playerRot = Player.transform.rotation.y;
        /*if (playerRot > 180) {
            playerRot -= 360;
        }*/

        transform.rotation = new Quaternion (0, playerRot+ initRotY, 0, transform.rotation.w); 
	}
}
