using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private int bananas;

    // Use this for initialization
    void Start()
    {
        this.setBananas(0);
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

}
