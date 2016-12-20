using UnityEngine;
using System.Collections;

public class VREnemyKrakeKopf : MonoBehaviour {

    Animator anim;
    int hit;
    int idle0StateHash;
    int idle1StateHash;
    int idle2StateHash;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        hit = Animator.StringToHash("hit");

        idle0StateHash = Animator.StringToHash("Base Layer.Kopf_idle_0");
        idle1StateHash = Animator.StringToHash("Base Layer.Kopf_idle_1");
        idle2StateHash = Animator.StringToHash("Base Layer.Kopf_idle_2");
    }
	    
	// Update is called once per frame
	void Update () {
	    
	}

    public void TakeDamage(int dmg) {
        transform.parent.parent.GetComponent<VREnemyKrake>().TakeDamage(dmg);
 
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == idle1StateHash || stateInfo.fullPathHash == idle2StateHash || stateInfo.fullPathHash == idle0StateHash)
        {
            anim.SetTrigger(hit);
        }
    }
}
