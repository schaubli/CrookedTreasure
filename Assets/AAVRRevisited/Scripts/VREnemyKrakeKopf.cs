using UnityEngine;
using System.Collections;

public class VREnemyKrakeKopf : MonoBehaviour {

    Animator anim;
    int hit;
    int die;
    int idle0StateHash;
    int idle1StateHash;
    int idle2StateHash;
    private bool dead;
    private VREnemyKrake parentKrake;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        hit = Animator.StringToHash("hit");
        die = Animator.StringToHash("die");

        idle0StateHash = Animator.StringToHash("Base Layer.Kopf_idle_0");
        idle1StateHash = Animator.StringToHash("Base Layer.Kopf_idle_1");
        idle2StateHash = Animator.StringToHash("Base Layer.Kopf_idle_2");

        parentKrake = transform.parent.parent.GetComponent<VREnemyKrake>();
        dead = false;
    }

    // Update is called once per frame
    private bool alreadyDying = false;
    void Update () {
        if (parentKrake.dead && alreadyDying == false)
        {
            anim.SetTrigger(die);
            alreadyDying = true;
        }
    }

    public void TakeDamage(int dmg) {
        parentKrake.TakeDamage(dmg);
        

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == idle1StateHash || stateInfo.fullPathHash == idle2StateHash || stateInfo.fullPathHash == idle0StateHash)
        {
            anim.SetTrigger(hit);
        }
    }
}
