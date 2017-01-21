using UnityEngine;
using System.Collections;

public class BananaVR : MonoBehaviour {
    Vector3 path;
    bool started = false;
    int counter = 0;
    // Use this for initialization
    public void StartFlying () {
       var target = new Vector3(-2, -1, -1);
       path = (target - transform.position) / 60;
       started = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (started)
        {
            counter++;
            transform.position += path;
            transform.Rotate(0, 5f, 0);

            if (counter == 60) {
                Player.Instance.addHealth(5);
                Destroy(this.gameObject); 
            }
        }
    }
}
