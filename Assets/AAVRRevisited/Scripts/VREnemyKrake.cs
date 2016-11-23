using UnityEngine;
using System.Collections;

public class VREnemyKrake : MonoBehaviour {

    public int health;
    public int attack;
    public int cooldown;
    public int cdcounter;
    public bool dead;

    // Use this for initialization
    void Start () {
        this.health = 100;
        this.attack = 10;
        this.cooldown = 180;
        this.cdcounter = 0;
        this.dead = false;
    }

    void Update() {
        this.cdcounter += 1;

        if (this.cdcounter == this.cooldown)
        {
            this.cdcounter = 0;
            this.Attack();
        }
    }

    void Attack() {
        // Start animation etc.
        Debug.Log("Krake attacks");
        Player.Instance.removeHealth(this.attack);
    }

    void Die() {

        Debug.Log("Krake died");
        // Destroy(this.gameObject); --> Handle in vrhandler
        this.dead = true;
        // Death Animation
    }



    public void TakeDamage(int dmg)
    {

        Debug.Log("Krake took dmg");
        this.health -= dmg;
        if (this.health <= 0)
        {
            this.Die();
        }
    }

}
