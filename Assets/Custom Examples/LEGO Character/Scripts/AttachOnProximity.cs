using UnityEngine;

public class AttachOnProximity : MonoBehaviour
{
    public GameObject objectToAttach; // The object to attach
    //public string handTag = "RightHand"; // Tag of the hand object
    [HideInInspector] public string handTag = "LeftHand"; // Tag of the hand object

    public float attachDistance = 1.0f; // Maximum distance for attachment
    public bool attachDuringAnimation = true; // Set true to check if animation is playing
    
    // Position and rotation offsets
    public Vector3 positionOffset = Vector3.zero; // Custom position offset (X, Y, Z)
    public Vector3 rotationOverride = Vector3.zero; // Custom rotation offsets (X, Y, Z)

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        // Find the child object with the specified tag
        Transform hand = FindChildWithTag(transform, handTag);


        // If not found search deeper into the model
        if (hand == null){
            // Recursively find the child object with the specified tag
            hand = FindChildWithTagRecursive(transform, handTag);

        }

        if (hand != null)
        {
            //AttachToHand(hand.gameObject);
            
            Debug.Log("Hand found with tag!");
            
             // Check distance between the object and the hand
            float distance = Vector3.Distance(hand.position, objectToAttach.transform.position);

            // Check if animation is playing (if required)
            bool isPlaying = animator != null && animator.GetCurrentAnimatorStateInfo(0).length > 0;

            Debug.Log($"Distance to hand: {distance}");

            if (distance <= attachDistance)
            {
            // Attach the object if conditions are met
            Debug.Log("Within attach distance!");

            if (!attachDuringAnimation || isPlaying)
                {
                    Debug.Log("Within attach distance!");
                    AttachToHand(hand.gameObject);
                }
                else {
                     Debug.Log("Within attach distance!");
                     AttachToHand(hand.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("No hand found with tag.");
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attachDistance);
    }

    private Transform FindChildWithTag(Transform parent, string tag)
    {
        // Iterate through all child objects to find the one with the specified tag
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child;
        }
        return null;
    }

    private Transform FindChildWithTagRecursive(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child;

            // Recursively search deeper levels
            Transform found = FindChildWithTagRecursive(child, tag);
            if (found != null)
                return found;
        }
        return null; // No matching tag found
    }


    private void AttachToHand(GameObject hand)
    {
        // Attach the object to the hand
        objectToAttach.transform.SetParent(hand.transform);

        // Optional: Set position and rotation to match hand
        //objectToAttach.transform.localPosition = Vector3.zero;
        //objectToAttach.transform.localRotation = Quaternion.identity;

        // Match hand's position and rotation
        //objectToAttach.transform.localPosition = Vector3.zero;
        //objectToAttach.transform.localRotation = Quaternion.Euler(rotationOverride) * hand.transform.localRotation;
        // Apply position and rotation offsets
        objectToAttach.transform.localPosition = positionOffset;
        objectToAttach.transform.localRotation = Quaternion.Euler(rotationOverride) * hand.transform.localRotation;

        Debug.Log($"{objectToAttach.name} attached to {hand.name} with position offset {positionOffset} and rotation override {rotationOverride}");
    }

      // Method to save offsets during play mode (called from the editor script)
    public void SaveOffsets()
    {
        if (objectToAttach != null)
        {
            positionOffset = objectToAttach.transform.localPosition;
            rotationOverride = objectToAttach.transform.localRotation.eulerAngles;
            Debug.Log($"Offsets saved: Position = {positionOffset}, Rotation = {rotationOverride}");
        }
        else
        {
            Debug.LogWarning("No object assigned to 'objectToAttach' to save offsets.");
        }
    }
}
