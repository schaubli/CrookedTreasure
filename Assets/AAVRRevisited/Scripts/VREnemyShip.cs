using UnityEngine;
using System.Collections;

public class VREnemyShip : MonoBehaviour {

    public float health;
    public bool dead;
    public GameObject cannonballsRightPrefab;
    private GameObject cannonballsRight;
    public GameObject cannonballsLeftPrefab;
    private GameObject cannonballsLeft;

    // Use this for initialization
    void Start () {
        this.dead = false;
    }
	
	// Update is called once per frame
	void Update () {
       
    }
    void ShootLeft()
    {
        Debug.Log("Ship attacks left!");
        cannonballsLeft = (GameObject)Instantiate(cannonballsLeftPrefab);
        Invoke("DestroyEmpty", 5);
    }
    void ShootRight()
    {
        Debug.Log("Ship attacks right!");
        cannonballsRight = (GameObject)Instantiate(cannonballsRightPrefab);
        Invoke("DestroyEmpty",5);
    }
    void DestroyEmpty() {
        
        if (cannonballsRight != null)
        {
            Destroy(cannonballsRight.gameObject);
            cannonballsRight = null;
        }
    }
    void Die()
    {
        this.dead = true;
    }

    public void TakeDamage(float dmg)
    {
        this.health -= dmg;
        if (this.health <= 0)
        {
            this.Die();
        }
        Debug.Log("Ship took " + dmg + " Damage");
    }
}
