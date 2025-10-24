using UnityEngine;
using Unity.LEGO.Behaviours.Triggers;
using Unity.LEGO.Behaviours;
using System.Collections.Generic;

namespace CustomNamespace
{
    public class CustomTouchTrigger : TouchTrigger
    {
        [Header("Touch Trigger Reference")]
        [Tooltip("Reference to the GameObject with the Trigger component.")]
        public GameObject triggerGameObject;

        private TouchTrigger triggerInstance;

        private bool eventCalled = false;


        // This method will be called when OnSensorActivated is triggered
        private void OnSensorActivated(SensoryCollider sensor, Collider other)
        {
            Debug.Log("Sensor Activated by: " + other.name + " on sensor: " + sensor.name);
        }

        // This method will be called when OnSensorDeactivated is triggered
        private void OnSensorDeactivated(SensoryCollider sensor)
        {
            Debug.Log("Sensor Deactivated: " + sensor.name);
        }



        protected void Start()
        {
            // Get the Trigger component from the referenced GameObject
            if (triggerGameObject != null)
            {
                triggerInstance = triggerGameObject.GetComponentInChildren<TouchTrigger>();

                if (triggerInstance == null)
                {
                    Debug.LogWarning("No TouchTrigger component found on the referenced GameObject.");
                }
                else
                {
                    Debug.Log("TouchTrigger instance successfully linked.");

                }
            }
            else
            {
                Debug.LogWarning("No GameObject assigned for the Touch Trigger reference.");
            }

             
        }

         private void OnEventTriggered()
        {
            eventCalled = true;
            Debug.Log("Event triggered!");
        }

       
    }
}
