using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour {

	Vector3 path; //Vector from start to end
	Vector3 startPosition; //start of animation
    bool started = false;
    int counter = 0; //current frame
	int duration; // in frames
	AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);

	public void FlyTowards(string gameObjectTag) {
		FlyTowards(GameObject.FindWithTag(gameObjectTag).transform.position, 180);
	}

	public void FlyTowards(Vector3 targetPosition, int duration) {
       this.path = (targetPosition - transform.position);
	   this.startPosition = this.transform.position;
	   this.duration = duration;
	   this.counter = 0;
       started = true;
       FindObjectOfType<VRhandler>().allowedToLeave = true;
	}
	

	void Update () {
        if (this.started)
        {
           this.counter++;
			transform.position = this.startPosition+(curve.Evaluate(this.counter*1f/this.duration)*this.path);
            transform.Rotate(0, 5f, 0);

            if (this.counter == duration) {
                Destroy(this.gameObject); 
            }
        }
    }
}
