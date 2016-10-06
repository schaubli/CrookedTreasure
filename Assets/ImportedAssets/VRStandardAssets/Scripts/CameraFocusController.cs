using UnityEngine;
using System.Collections;
using Vuforia;

public class CameraFocusController : MonoBehaviour {

	public CameraDevice.FocusMode focusMode = CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO;

	void Start () 
	{
	    VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
	    VuforiaBehaviour.Instance.RegisterOnPauseCallback(OnPaused);
	}

	void OnEnable() {
		CameraDevice.Instance.SetFocusMode(focusMode);
	}
	  
	private void OnVuforiaStarted()
	{
	    CameraDevice.Instance.SetFocusMode(focusMode);
	}
	  
	private void OnPaused(bool paused)
	{
	    if (!paused) // resumed
	    {
	        // Set again autofocus mode when app is resumed
	        CameraDevice.Instance.SetFocusMode(focusMode);
	    }
	}
}
