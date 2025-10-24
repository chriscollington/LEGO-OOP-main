using UnityEngine;

public class SetYTransform : MonoBehaviour
{
    public RectTransform targetObject; // The object whose Y you want to set
    public RectTransform referenceObject; // The object to use as a reference

    void Update()
    {
        if (targetObject != null && referenceObject != null)
        {
            // Get the current position of the target object
            Vector3 targetPosition = targetObject.position;

            // Set the Y position based on the reference object's Y position
            targetPosition.y = referenceObject.position.y;

            // Apply the updated position back to the target object
            targetObject.position = targetPosition;
        }
    }
}
