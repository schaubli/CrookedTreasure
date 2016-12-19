using UnityEngine;
using System.Collections;



public class Canon : MonoBehaviour {

    public GameObject Player;
    public GameObject Cannonball;
    public GameObject vrHandler;
    public int damage;
    private GameObject cannonballObject;
    public int cooldown;
    private int cooldowncounter;
    private float rotationOffset;


    // Use this for initialization
    void Start () {
        rotationOffset = +90;
        cooldowncounter = 0;
        cannonballObject = null;
        if (Cannonball == null)
        {
            Debug.LogError("Cannonball is not defined in Canon.cs");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (vrHandler.GetComponent<VRhandler>().mode == 0 || vrHandler.GetComponent<VRhandler>().mode == 2)
        {
            var playerEulerY = Player.transform.eulerAngles.y;
            if (playerEulerY < 180)
            {
                Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, playerEulerY + rotationOffset, transform.eulerAngles.z);
                transform.rotation = Quaternion.Euler(eulerRotation);
            }
            if (this.cooldowncounter == cooldown)
            {
                this.Shoot();
                this.cooldowncounter = 0;
            }

            

            cooldowncounter += 1;
        }
        else
        {
            if (cannonballObject != null)
            {
                Destroy(cannonballObject);
            }
        }
    }

    void Shoot() {
        if (cannonballObject != null) {
            Destroy(cannonballObject);
        }
        cannonballObject = (GameObject)Instantiate(Cannonball,transform.position, Quaternion.identity);
        Canonball cannonballScript = cannonballObject.GetComponent<Canonball>();
        
        cannonballScript.SetRotation(this.transform.rotation.eulerAngles.y, Player.transform.rotation.eulerAngles.x);
        cannonballScript.SetPosition(this.transform.position);
        cannonballScript.SetDamage(this.damage);
        cannonballScript.Fire();
        
       
        GetComponent<Animation>().Play("CannonRecoil");

    }

}
