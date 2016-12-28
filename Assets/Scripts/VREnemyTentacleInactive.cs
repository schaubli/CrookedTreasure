using UnityEngine;
using System.Collections;

public class VREnemyTentacleInactive : MonoBehaviour {

    int die;
    Animator anim;

    private VREnemyKrake parentKrake;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        die = Animator.StringToHash("die");
        parentKrake = transform.parent.GetComponent<VREnemyKrake>();
    }

    // Update is called once per frame
    private bool alreadyDying = false;
    void Update()
    {
        if (parentKrake.dead && alreadyDying == false)
        {
            anim.SetTrigger(die);
            alreadyDying = true;
        }
    }
}
