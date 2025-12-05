using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class AIWander: MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 6f;
    public float gravity = -9.81f;

    [Header("Walkable LEGO Objects")]
    [Tooltip("Drag any LEGO object (floor, baseplate, terrain, etc.)")]
    public List<GameObject> walkableObjects;

    [Header("Obstacles (AI will avoid these)")]
    [Tooltip("Drag objects like plants, rocks, props that AI should avoid.")]
    public List<GameObject> obstacles;

    [Header("Animation")]
    public Animator legoAnimator;
    public string speedParameter = "Speed";
    public float animationSpeedMultiplier = 2f;

    private CharacterController controller;
    private Vector3 targetPosition;
    private Vector3 verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (walkableObjects.Count == 0)
            Debug.LogWarning("No walkable LEGO objects assigned!");

        ChooseNewDestination();
    }

    void Update()
    {
        MoveAI();

        Vector3 flatAI = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatTarget = new Vector3(targetPosition.x, 0, targetPosition.z);

        if (Vector3.Distance(flatAI, flatTarget) < 1f)
        {
            ChooseNewDestination();
        }
    }

    void ChooseNewDestination()
    {
        if (walkableObjects.Count == 0)
        {
            targetPosition = transform.position;
            return;
        }

        int attempts = 0;
        bool validPoint = false;

        while (!validPoint && attempts < 20)
        {
            attempts++;

            // Pick random walkable object
            GameObject chosenObj = walkableObjects[Random.Range(0, walkableObjects.Count)];
            Collider[] colliders = chosenObj.GetComponentsInChildren<Collider>();

            if (colliders.Length == 0)
            {
                targetPosition = transform.position;
                return;
            }

            Collider randomCollider = colliders[Random.Range(0, colliders.Length)];
            Bounds b = randomCollider.bounds;

            float x = Random.Range(b.min.x, b.max.x);
            float z = Random.Range(b.min.z, b.max.z);

            // Raycast down to get correct Y
            RaycastHit hit;
            if (!Physics.Raycast(new Vector3(x, b.max.y + 2, z), Vector3.down, out hit, 200f))
            {
                continue;
            }

            Vector3 potentialPos = hit.point;

            // Check if inside any obstacle
            bool hitObstacle = false;
            foreach (GameObject obs in obstacles)
            {
                if (obs == null) continue;

                Collider[] obsCols = obs.GetComponentsInChildren<Collider>();
                foreach (Collider c in obsCols)
                {
                    if (c.bounds.Contains(potentialPos))
                    {
                        hitObstacle = true;
                        break;
                    }
                }
                if (hitObstacle) break;
            }

            if (!hitObstacle)
            {
                targetPosition = potentialPos;
                validPoint = true;
            }
        }

        // If no valid point found after 20 tries, stay put
        if (!validPoint)
            targetPosition = transform.position;
    }

    void MoveAI()
    {
        Vector3 moveDirection = targetPosition - transform.position;
        moveDirection.y = 0f;
        moveDirection = moveDirection.normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        if (controller.isGrounded)
            verticalVelocity.y = -1f;
        else
            verticalVelocity.y += gravity * Time.deltaTime;

        Vector3 movement = (moveDirection * moveSpeed) + verticalVelocity;
        controller.Move(movement * Time.deltaTime);

        // Animation
        if (legoAnimator != null)
        {
            float currentSpeed = moveDirection.magnitude * moveSpeed;
            float animSpeed = currentSpeed * animationSpeedMultiplier;
            legoAnimator.SetFloat(speedParameter, animSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (walkableObjects != null)
        {
            foreach (GameObject obj in walkableObjects)
            {
                if (obj != null)
                {
                    foreach (Collider col in obj.GetComponentsInChildren<Collider>())
                    {
                        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
                    }
                }
            }
        }

        Gizmos.color = Color.red;
        if (obstacles != null)
        {
            foreach (GameObject obs in obstacles)
            {
                if (obs != null)
                {
                    foreach (Collider col in obs.GetComponentsInChildren<Collider>())
                    {
                        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
                    }
                }
            }
        }
    }
}