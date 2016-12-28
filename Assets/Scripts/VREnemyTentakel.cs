using UnityEngine;
using System.Collections;

public class VREnemyTentakel : MonoBehaviour {

    public int assign = 0;
    Animator anim;
    int attack1;
    int attack2;
    int hit;
    int die;
    VREnemyKrake parentKrake;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

        die = Animator.StringToHash("die");
        hit = Animator.StringToHash("hit");
        attack1 = Animator.StringToHash("attack1");
        attack2 = Animator.StringToHash("attack2");


        parentKrake = GetComponentInParent<VREnemyKrake>();
    }
    private bool alreadyDying = false;
	// Update is called once per frame
	void Update () {
        if (parentKrake.dead && alreadyDying == false)
        {
            anim.SetTrigger(die);
            alreadyDying = true;
        }

    }

    public void PlayAnimation(int animnr) {
        
        switch (animnr)
        {
            case 0: break;
            case 1:
                anim.SetTrigger(attack1);
                break;
            case 2:
                anim.SetTrigger(attack2);
                break;
            default: break;
        }
            

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cannonball"))
        {
            anim.SetTrigger(hit);
            Destroy(other.gameObject);
            Debug.Log("Tentakel was hit");
            parentKrake.TakeDamage(other.gameObject.GetComponent<Canonball>().damage / 2);

        }
    }

    void DoDamage (){
        Player.Instance.removeHealth(parentKrake.attack);
        Debug.Log("Player took damage");
    }
    


}
