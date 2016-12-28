using UnityEngine;
using System.Collections;

public class VRhandler : MonoBehaviour {
    [HideInInspector]
    public int mode;
    [HideInInspector]
    public int enemy;

    public GameObject monsterPrefab;
    private VREnemyKrake krake = null;
    public GameObject cameraRig;
    // public GameObject shipPrefab;

    public GameObject lootCratePrefab;

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
                        // Set position and rotation of camera
                        cameraRig.transform.position = new Vector3(-1.79f, -2.553f, -1.459f);
                        Vector3 cameraRigEulerRotation = new Vector3(0, 90, 0);
                        cameraRig.transform.rotation = Quaternion.Euler(cameraRigEulerRotation);

                        // init monster
                        GameObject monsterVRGameObject = (GameObject)Instantiate(monsterPrefab);
                        krake = monsterVRGameObject.GetComponent<VREnemyKrake>();
  
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

    void initLooting() {
        this.mode = 2;
        Debug.Log("Mode switched to Looting");

        if (krake != null)
        {
           
            Destroy(krake.gameObject);
            
        }
        // Set position and rotation of camera
        // cameraRig.transform.position = new Vector3(-3.56f, 0.59f, -13.42f);
        // Vector3 cameraRigEulerRotation = new Vector3(0, 180, 0);
        // cameraRig.transform.rotation = Quaternion.Euler(cameraRigEulerRotation);

        //Invoke("endVR", 15);
        this.spawnLoot();
    }

    void spawnLoot() {
        int rndNum = (int)Random.Range(3, 6);
        Debug.Log(rndNum + " Lootcrates spawned");
        for (int i = 0; i < rndNum; i++)
        {
            //x : zwischen 9 und 15
            //y : -6
            //z : zwischen -7 und 7
            Vector3 rndPosition = new Vector3(Random.value * 6 + 9, -6, Random.value * 14 - 7);
            GameObject lootCrateObject = (GameObject)Instantiate(lootCratePrefab);
            // LootCrate lootCrateScript = lootCrateObject.GetComponent<LootCrate>();
            lootCrateObject.gameObject.transform.position = rndPosition;
        }
    }

    private bool alreadydead = false;
	// Update is called once per frame
	void Update () {

        if (alreadydead == false)
        {
            if (krake != null && krake.dead == true)
            {
                Invoke("initLooting", 3);
                alreadydead = true;
            }
        }
     

	}

    void endVR()
    {
        this.mode = 99;
        GameObject[] leftover_lootcrates = GameObject.FindGameObjectsWithTag("LootCrate");

        foreach (GameObject lootcrate in leftover_lootcrates)
        {
            Destroy(lootcrate);
        }

        PlayerController.Instance.EndVRMode();
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
