using UnityEngine;
using Unity.LEGO.Behaviours.Triggers;
using Unity.LEGO.Game;
using Unity.VisualScripting;

namespace CustomNamespace
{
    public class CustomNearbyTrigger : MonoBehaviour
    {
        [Header("Nearby Trigger Reference")]
        [Tooltip("Reference to the GameObject with the trigger component.")]
        public GameObject triggerGameObject;

        private NearbyTrigger triggerInstance;

        protected void Start()
        {
            // Get the NearbyTrigger component from the referenced GameObject
            if (triggerGameObject != null)
            {
                triggerInstance = triggerGameObject.GetComponentInChildren<NearbyTrigger>();

                if (triggerInstance == null)
                {
                    Debug.LogWarning("No Nearby Trigger component found on the referenced GameObject.");
                }
                else
                {
                    Debug.Log("Nearby Trigger instance successfully linked.");
                    Debug.Log(triggerInstance.Goal.ToString());
                   
                }
            }
            else
            {
                Debug.LogWarning("No GameObject assigned for the Nearby Trigger reference.");
            }
        }
       
        #region Exposing Events and Methods from Trigger

        // Method to trigger Reset of Trigger
        public void TriggerReset()
        {
            if (triggerInstance != null)
            {
                //Reset(); // Call the Reset method of the referenced Trigger
                Debug.Log("Reset method called on Nearby Trigger.");
            }
            else
            {
                Debug.LogWarning("Nearby Trigger instance is not assigned.");
            }
        }

        // Example: Exposing the Trigger Broadcast method from NearbyTrigger
        public void TriggerBroadcast(GameEvent gameEvent)
        {
            if (triggerInstance != null)
            {
                EventManager.Broadcast(gameEvent); // Assuming you're using EventManager to broadcast events
                Debug.Log("Broadcasting event from NearbyTrigger." +gameEvent.ToString() );
            }
            else
            {
                Debug.LogWarning("NearbyTrigger instance is not assigned.");
            }
        }

        // Additional exposed method for flexibility
        public void CallTriggerMethodExample()
        {
            if (triggerInstance != null)
            {
                // Example of calling any method from the referenced NearbyTrigger, if needed
                // For example: nearbyTriggerInstance.SomeMethod();
                Debug.Log("Method called on the referenced NearbyTrigger.");
            }
            else
            {
                Debug.LogWarning("NearbyTrigger instance is not assigned.");
            }
        }

        #endregion
 
    }
}
