using UnityEngine;
using System.Collections;

public class CanonHalterung : MonoBehaviour {

    public GameObject Player;
    public GameObject vrHandler;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (vrHandler.GetComponent<VRhandler>().mode == 0 || vrHandler.GetComponent<VRhandler>().mode == 2)
        {
            var playerEulerX = Player.transform.eulerAngles.x;
            //if (playerEulerX < 0)
            //{
            Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, playerEulerX);
            transform.rotation = Quaternion.Euler(eulerRotation);
            //}
        }
    }
}
