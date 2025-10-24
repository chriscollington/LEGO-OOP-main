using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationDampener : MonoBehaviour
{
    public Animator animator;
    public float dampeningFactor = 0.5f; // Adjust this value to dampen the movements
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Dampen position
        Vector3 currentPosition = transform.position;
        Vector3 dampenedPosition = Vector3.Lerp(lastPosition, currentPosition, dampeningFactor);
        transform.position = dampenedPosition;

        // Dampen rotation
        Quaternion currentRotation = transform.rotation;
        Quaternion dampenedRotation = Quaternion.Lerp(lastRotation, currentRotation, dampeningFactor);
        transform.rotation = dampenedRotation;

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    public void SetDampeningFactor(float newDampeningFactor)
    {
        dampeningFactor = Mathf.Clamp(newDampeningFactor, 0.0f, 1.0f);
    }
}

