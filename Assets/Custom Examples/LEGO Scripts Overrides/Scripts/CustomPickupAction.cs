using UnityEngine;
using Unity.LEGO.Behaviours.Triggers;
using Unity.LEGO.Behaviours.Actions;
using Unity.LEGO.Game;


namespace CustomNamespace
{
    public class CustomPickupAction : MonoBehaviour
    {
        [Header("Touch Trigger Reference")]
        [Tooltip("Reference to the GameObject with the Trigger component.")]
        public GameObject actionGameObject;

        private PickupAction actionInstance;

        protected void Start()
        {
            // Get the Action  component from the referenced GameObject
            if (actionGameObject != null)
            {
                actionInstance = actionGameObject.GetComponentInChildren<PickupAction>();

                if (actionInstance == null)
                {
                    Debug.LogWarning("No PickupAction component found on the referenced GameObject.");
                }
                else
                {
                    Debug.Log("Pickup Action instance successfully linked.");
                     Debug.Log(actionInstance.name.ToString());
                   
                }
            }
            else
            {
                Debug.LogWarning("No GameObject assigned for the Pickup Action reference.");
            }
        }
       
        #region Exposing Events and Methods from Action

        // Method to trigger Reset of Trigger
        public void TriggerReset()
        {
            if (actionInstance != null)
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
        public void ActionBroadcast(GameEvent gameEvent)
        {
            if (actionInstance != null)
            {
                EventManager.Broadcast(gameEvent); // Assuming you're using EventManager to broadcast events
                Debug.Log("Broadcasting event from NearbyTrigger.");
            }
            else
            {
                Debug.LogWarning("NearbyTrigger instance is not assigned.");
            }
        }

        // Additional exposed method for flexibility
        public void CallActionMethodExample()
        {
            if (actionInstance != null)
            {
                // Example of calling any method from the referenced NearbyTrigger, if needed
                // For example: nearbyTriggerInstance.SomeMethod();
                Debug.Log("Method called on the referenced PickupAction.");
            }
            else
            {
                Debug.LogWarning("PickupAction instance is not assigned.");
            }
        }

        #endregion
 
    }
}
