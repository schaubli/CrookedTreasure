/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);
            Light[] lightComponents = GetComponentsInChildren<Light>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            EntityMover[] movingObjects = TileManager.Instance.moversInScene;

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            foreach (Canvas component in canvasComponents)
            {
                component.enabled = true;
            }

            foreach (Light component in lightComponents)
            {
                component.enabled = true;
            }


            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            foreach (EntityMover mover in movingObjects) {
            	Debug.Log("Checking moving Object "+mover.gameObject.name);
            	if(mover.currentTile.IsShown == false ) {
					Debug.Log("Hiding moving Object "+mover.gameObject.name);
            		Renderer[] meshsInMovingObject = mover.gameObject.GetComponentsInChildren<Renderer>(true);
            		foreach (Renderer component in meshsInMovingObject)
					{
						component.enabled = false;
						Debug.Log("Disabling "+component.gameObject.name);
					}
            	}
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);
            Light[] lightComponents = GetComponentsInChildren<Light>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            foreach (Canvas component in canvasComponents)
            {
                component.enabled = false;
            }

            foreach (Light component in lightComponents)
            {
                component.enabled = false;
            }


            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
