using UnityEngine;
using System.Collections;

public class LootCrate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.rotation = new Quaternion(Random.value / 5 ,Random.value * 3.14f*2,Random.value / 5, this.transform.rotation.w);
       
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LootCrate"))
        {
            if (other.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                Destroy(this.gameObject);
                Debug.Log("Destroyed overlapping Lootcrate");
            }
        }
    }

    public void Hit() {
        Destroy(this.gameObject);
    }
}
