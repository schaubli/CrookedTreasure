using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IPlayerEventTarget : IEventSystemHandler
{
    // functions that can be called via the messaging system
    void UpdatePlayer();
}

public class Player : MonoBehaviour {

    public static Player Instance;

    private int health;
    private int maxHealth;

    // Use this for initialization
    void Start()
    {
        this.setHealthMax(PlayerController.Instance.playerMaxHealth);
        this.setHealth(PlayerController.Instance.playerStartHealth);
    }

    public int getHealth() {
        return this.health;
    }

    public int getMaxHealth() {
        return this.maxHealth;
    }

    public void addHealth(int amount) {
        this.health += amount;
        if(this.health >this.maxHealth) {
            this.setHealth(this.maxHealth);
        }
        ExecuteEvents.Execute<IPlayerEventTarget>(gameObject, null, (x,y)=>x.UpdatePlayer());
    }

    public void removeHealth(int amount) {
        this.health -= amount;
        ExecuteEvents.Execute<IPlayerEventTarget>(gameObject, null, (x,y)=>x.UpdatePlayer());
    }

    public void setHealth(int amount) {
        this.health = amount;
        if(this.health >this.maxHealth) {
            this.setHealth(this.maxHealth);
        }
        ExecuteEvents.Execute<IPlayerEventTarget>(gameObject, null, (x,y)=>x.UpdatePlayer());
    }

    public void addHealthMax(int amount) {
        this.maxHealth += amount;
        ExecuteEvents.Execute<IPlayerEventTarget>(gameObject, null, (x,y)=>x.UpdatePlayer());
    }

    public void removeHealthMax(int amount) {
        this.maxHealth -= amount;
        if(this.health >this.maxHealth) {
            this.setHealth(this.maxHealth);
        }
        ExecuteEvents.Execute<IPlayerEventTarget>(gameObject, null, (x,y)=>x.UpdatePlayer());
    }

    public void setHealthMax(int amount) {
        this.maxHealth = amount;
        if(this.health >this.maxHealth) {
            this.setHealth(this.maxHealth);
        }
        ExecuteEvents.Execute<IPlayerEventTarget>(gameObject, null, (x,y)=>x.UpdatePlayer());
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

    public void OnAnimationEnded() {
        PlayerController.Instance.OnAnimationEnded();
    }

}
