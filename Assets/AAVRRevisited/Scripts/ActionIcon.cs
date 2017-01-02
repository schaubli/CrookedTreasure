using UnityEngine;
using System.Collections;
public class ActionIcon : MonoBehaviour {

    VRhandler vrHandler = null;
    ActionIconType type;
    public enum ActionIconType
    {
        Crosshair,
        Steeringwheel
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0,1,0);
	}
    public void setType (ActionIconType inputtype)
    {
        this.type = inputtype;
    }
    public void setVrHandler(GameObject vrhandlerGO)
    {
        this.vrHandler = vrhandlerGO.GetComponent<VRhandler>();
    }

    public void activate() {
        if (this.vrHandler != null) {
            this.vrHandler.actionIconEvent(this.type);
            Destroy(this.gameObject);
        }
    }
}
