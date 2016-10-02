using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Player Instance;

    private int bananas;

    // Use this for initialization
    void Start()
    {
        this.setBananas(50);
    }

    public int getBananas() {
        return this.bananas;
    }

    public void addBananas(int amount) {
        this.bananas += amount;
    }

    public void removeBananas(int amount) {
        this.bananas -= amount;
    }

    public void setBananas(int amount) {
        this.bananas = amount;    
    }

    void Awake()
    {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

}
