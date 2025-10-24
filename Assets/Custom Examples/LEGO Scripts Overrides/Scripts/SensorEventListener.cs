using UnityEngine;
using Unity.LEGO.Behaviours.Triggers;
using Unity.LEGO.Behaviours;
using System.Collections.Generic;
using Unity.LEGO.Game;
using Unity.VisualScripting;

public class SensorEventListener : MonoBehaviour
{
    public GameObject triggerInstance; // Reference to the GameObject with the TouchTrigger

    private TouchTrigger touchTrigger;

    private void Start()
    {
        // Attempt to find a TouchTrigger in the referenced GameObject or its children.
        touchTrigger = triggerInstance.GetComponentInChildren<TouchTrigger>();

        if (touchTrigger != null)
        {
            // Subscribe to events in TouchTrigger
            SubscribeToTouchTriggerEvents();
        }
        else
        {
            Debug.Log("TouchTrigger is not found on this GameObject or its children.");
        }

        // Attempt to get the NearbyTrigger component (which inherits Trigger)
        touchTrigger = GetComponentInParent<TouchTrigger>();

        if (touchTrigger != null)
        {
            // Subscribe to OnActivate and OnProgress events from NearbyTrigger (which inherits from Trigger)
            SubscribeToTriggerEvents();
        }
        else
        {
            Debug.Log("Touch Trigger (or Trigger) not found on this GameObject or its parents.");
        }
    }

    private void Updated(){

            GameOverEvent evt = Events.GameOverEvent;
            evt.Win = false;
            EventManager.Broadcast(evt);
            Debug.Log($"EVENT: {evt.ToString()}");

    }
    private void SubscribeToTouchTriggerEvents()
    {
        Debug.Log("TouchTrigger Events.");
        // Iterate over the active sensory colliders in the TouchTrigger
        foreach (var sensoryCollider in touchTrigger.ActiveColliders)
        {
            sensoryCollider.OnSensorActivated += OnSensorActivated;
            sensoryCollider.OnSensorDeactivated += OnSensorDeactivated;
            Debug.Log($"Subscribed to SensoryCollider events from: {sensoryCollider.name}");
        }
    }

    private void SubscribeToTriggerEvents()
    {
        // Subscribe to OnActivate and OnProgress events from NearbyTrigger (which inherits Trigger)
        touchTrigger.OnActivate += HandleOnActivate;
        touchTrigger.OnProgress += HandleOnProgress;
    }

    private void OnDestroy()
    {
        // Unsubscribe from SensoryCollider events
        if (touchTrigger != null)
        {
            foreach (var sensoryCollider in touchTrigger.ActiveColliders)
            {
                sensoryCollider.OnSensorActivated -= OnSensorActivated;
                sensoryCollider.OnSensorDeactivated -= OnSensorDeactivated;
                Debug.Log($"Unsubscribed from SensoryCollider events from: {sensoryCollider.name}");
            }
        }

        // Unsubscribe from OnActivate and OnProgress events from NearbyTrigger (which inherits Trigger)
        if (touchTrigger != null)
        {
            touchTrigger.OnActivate -= HandleOnActivate;
            touchTrigger.OnProgress -= HandleOnProgress;
        }
    }

    // Event handler for Sensor Activation
    private void OnSensorActivated(SensoryCollider sensor, Collider other)
    {
        Debug.Log($"Sensor Activated! Sensor: {sensor.name}, Triggered by: {other.name}");
    }

    // Event handler for Sensor Deactivation
    private void OnSensorDeactivated(SensoryCollider sensor)
    {
        Debug.Log($"Sensor Deactivated! Sensor: {sensor.name}");
    }

    // Event handler for OnActivate from Trigger
    private void HandleOnActivate()
    {
        Debug.Log("Trigger Activated (OnActivate event)");
    }

    // Event handler for OnProgress from Trigger
    private void HandleOnProgress()
    {
        Debug.Log("Progress Updated (OnProgress event)");
    }
}
