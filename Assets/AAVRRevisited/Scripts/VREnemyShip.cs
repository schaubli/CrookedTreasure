using UnityEngine;
using System.Collections;

public class VREnemyShip : MonoBehaviour {

    public float health;
    public int attack;
    public int cooldown;
    public int cdcounter;
    public bool dead;
    // Use this for initialization
    void Start () {
        this.cdcounter = 0;
        this.dead = false;
    }
	
	// Update is called once per frame
	void Update () {
        this.cdcounter += 1;

        if (this.cdcounter == this.cooldown)
        {
            this.cdcounter = 0;
            this.Action();
        }
    }
    void Action()
    {
        Debug.Log("Ship attacks!");
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
