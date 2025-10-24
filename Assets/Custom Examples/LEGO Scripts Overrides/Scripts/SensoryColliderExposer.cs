using UnityEngine;
using Unity.LEGO.Behaviours.Triggers;
using System.Collections.Generic;
using Unity.LEGO.Behaviours;

public class SensoryColliderExposer : MonoBehaviour
{
    [Tooltip("Reference to the TouchTrigger.")]
    public TouchTrigger touchTrigger;

    private List<SensoryCollider> sensoryColliders = new List<SensoryCollider>();

    public IReadOnlyList<SensoryCollider> SensoryColliders => sensoryColliders;

    private void Start()
    {
        if (touchTrigger != null)
        {
        

            if (sensoryColliders.Count == 0)
            {
                Debug.LogWarning("No SensoryColliders were found in the TouchTrigger.");
            }
            else
            {
                Debug.Log($"Found {sensoryColliders.Count} SensoryColliders.");
            }
        }
        else
        {
            Debug.LogError("TouchTrigger is not assigned in SensoryColliderExposer.");
        }
    }
}
