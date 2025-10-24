using Unity.VisualScripting;
using UnityEngine;
using Unity.LEGO.Minifig;

[RequireComponent(typeof(Animator))]
public class AnimationVelocityController : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;

    public PlayerUIHandler player;

    public EnemyAI enemyAI;

    private Animator playerAnimator;




    // Animator parameter names
    private static readonly int VelocityParam = Animator.StringToHash("Velocity");
    private static readonly int AttackParam = Animator.StringToHash("Attack");
    private static readonly int SpecialParam = Animator.StringToHash("Play Special");


    // Player reference

    public float attackRange = 1.5f;

    void Awake()
    {
        // Get the required components
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }

        // Initialize the last position to the current position
        lastPosition = transform.position;
    }

    void Update()
    {
        if (animator == null) return;

        // Calculate the velocity based on the change in position
        Vector3 currentPosition = transform.position;
        Vector3 displacement = currentPosition - lastPosition;
        float speed = displacement.magnitude / Time.deltaTime;

        // Update the last position
        lastPosition = currentPosition;

        // Clamp the speed between 0 and 1
        speed = Mathf.Clamp(speed, 0f, 1f);

        // Map the speed to idle (0.2 and below) and walking (0.3 and above)
        if (speed <= 0.2f)
        {
            speed = 0f; // Idle
        }
        else if (speed >= 0.3f)
        {
            speed = 1f; // Walking
        }

        // Set the velocity parameter in the animator
        animator.SetFloat(VelocityParam, speed);

        // Check if the GameObject is within attack range of the player
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= attackRange)
            {
                animator.SetBool(AttackParam, true);

                player.playerInside = true;
                // Show the warning panel
                player.ShowWarningPanel();

                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
                {
                    Debug.Log("Special animation key pressed.");
                    // Take damage to the enemy
                    enemyAI.TakeDamage(5);
                    // Get and play the player animator
                    playerAnimator = player.GetComponent<Animator> ();
                    playerAnimator.SetTrigger(SpecialParam);
                }

            }
            else
            {
                animator.SetBool(AttackParam, false);

                player.playerInside = false;   
                // Hide the warning panel 
                player.HideWarningPanel();
            }
        }


    }

    void OnDrawGizmosSelected()
    {
        // Draw the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
