using UnityEngine;

public class MinifigureAttack : MonoBehaviour
{
    private Animator animator;

    private GameObject currentTarget;

    // Animator parameter names
    private static readonly int SpecialParam = Animator.StringToHash("Play Special");

    // Health management
    public int specialDamage = 5;

    void Awake()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }
    }

    void Update()
    {
        /*
        if (animator == null ) return;

          if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Special animation key pressed.");


            // Trigger the special animation
            animator.SetTrigger(SpecialParam);

            // Deduct health from the current target if it has an EnemyHealth component
            EnemyHealth targetHealth = currentTarget.GetComponent<EnemyHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(specialDamage);
            }
        }
        */
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Set the current target to the object this GameObject has collided with
        currentTarget = collision.gameObject;

        // Output a debug message if the collided object has the tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with enemy: " + collision.gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Clear the current target when the collision ends
        if (collision.gameObject == currentTarget)
        {
            currentTarget = null;
        }
    }
}

