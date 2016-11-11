using UnityEngine;
using System.Collections;

public class VRhandler : MonoBehaviour {
    [HideInInspector]
    public int mode;
    [HideInInspector]
    public int enemy;

    public GameObject monsterPrefab;
    // public GameObject shipPrefab;

    void Start()
    {
        if (monsterPrefab == null)
        {
            Debug.LogError("monsterPrefab is not defined in VRhandler");
        }
    }

    public void initVR () {
       
        switch (this.mode)
        {
            case 0:     // Fight

                Debug.Log("Initiate VR Fight");

                switch (this.enemy) {

                    case 0: //Monster 

                        GameObject monsterVRGameObject = (GameObject)Instantiate(monsterPrefab);
                        VREnemyKrake krake = monsterVRGameObject.GetComponent<VREnemyKrake>();
                        

                        break;
                    case 1: //Ship
                        break;
                    default:
                        break;

                }

                break;

            case 1:
                Debug.Log("Initiate VR Island");
                break;
            default: break;
        }



        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /*
    private static VRhandler instance;
    public static VRhandler Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = (VRhandler)obj.AddComponent(typeof(VRhandler));
            }
            return instance;
        }
    }*/
}
