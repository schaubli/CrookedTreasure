using UnityEngine;
using System.Collections;

public class Harpune : MonoBehaviour {

    public GameObject vrHandler;
    public GameObject Player;


	// Update is called once per frame
	void Update () {
        if(vrHandler.GetComponent<VRhandler>().mode == 2)
        {
            var playerEulerY = Player.transform.eulerAngles.y;
            Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, playerEulerY, transform.eulerAngles.z);
            transform.rotation = Quaternion.Euler(eulerRotation);
            Debug.Log("Harpune activated");
        }
    }
}
