using UnityEngine;
using System.Collections;

public class Canonball : MonoBehaviour {

    private Rigidbody rb;
    private int damage;

    void Start() {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.position = transform.position;
    }

    public void SetPosition(Vector3 cannonPos) {
        this.transform.position = new Vector3(cannonPos.x,cannonPos.y, cannonPos.z);
    }
    public void SetRotationY(float cannonEulerAnglesY) {
        Vector3 eulerRotation = new Vector3(-40, cannonEulerAnglesY + 90, 0);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }

    public void SetDamage(int dmg) {
        this.damage = dmg;
    }

    public void Fire(){

       // rb.AddForce(transform.forward * 1, ForceMode.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
     this.transform.position = this.transform.position + transform.forward * 0.5f;
        this.transform.Rotate(1.5f, 0, 0);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("VREnemy"))
        {
            Debug.Log("Hit");
            other.gameObject.GetComponent<VREnemyKrake>().TakeDamage(this.damage);
            Destroy(this.gameObject);
        }
    }
}
