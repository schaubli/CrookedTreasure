using UnityEngine;
using System.Collections;

public class VREnemyKrake : MonoBehaviour {

    public float health;
    public int attack;
    public int cooldown;
    public int cdcounter;
    public bool dead;

    VREnemyTentakel[] tentakel;


    void Start () {
        /*
        this.health = 100;
        this.attack = 10;
        this.cooldown = 180;*/
        this.cdcounter = 0;
        this.dead = false;

        tentakel = this.gameObject.GetComponentsInChildren<VREnemyTentakel>();

    }

    void Update() {
        this.cdcounter += 1;

        if (this.cdcounter == this.cooldown)
        {
            this.cdcounter = 0;
            this.Action();
        }
    }

    void Action() {
        // Start animation etc.
        int ActionNr = (int)Mathf.Round(Random.value) + 1;
        Debug.Log(ActionNr);

        foreach (VREnemyTentakel et in tentakel)
        {
            et.PlayAnimation(ActionNr);
        }

        //tentakel1.PlayAnimation(ActionNr);

        //Player.Instance.removeHealth(this.attack);
    }

    void Die() {

        Debug.Log("Krake died");
        // Destroy(this.gameObject); --> Handle in vrhandler
        this.dead = true;
        // Death Animation
    }



    public void TakeDamage(float dmg)
    {

        Debug.Log("Krake took dmg");
        this.health -= dmg;
        if (this.health <= 0)
        {
            this.Die();
        }
    }

}
