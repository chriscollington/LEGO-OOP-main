using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace Unity.LEGO.Minifig
{
public class EnemyAI : MonoBehaviour
{
     [Header("Settings")]
     private NavMeshAgent navAgent;


    // Player reference
    public Transform player;
    private PlayerUIHandler playerUIHandler;

    private EnemyExplode enemyExplode;

    public LayerMask groundLayer, playerLayer;
    public float health;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float sightRange;
    public float attackRange = 1.5f;
    public int damage;
    private Animator animator;
    private ParticleSystem hitEffect;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool takeDamage;

    [Header("Custom")]
    private Vector3 lastPosition;
    private Animator playerAnimator;

    // Animator parameter names
    private static readonly int VelocityParam = Animator.StringToHash("Velocity");
    private static readonly int AttackParam = Animator.StringToHash("Attack");
    private static readonly int SpecialParam = Animator.StringToHash("Play Special");
    private static readonly int specialIdHash = Animator.StringToHash("Special Id");


    Rigidbody rb;

    private void Awake()
    {
        animator = GetComponent<Animator> ();

        if (animator == null)
        {
           Debug.LogError("Animator component not found on the GameObject.");
        }

        navAgent = GetComponent<NavMeshAgent>();

        if (navAgent == null)
        {
           Debug.LogError("NavAgent component not found on the GameObject.");
        }

        playerUIHandler = player.GetComponent<PlayerUIHandler>();

        if (playerUIHandler == null)
        {
           Debug.LogError("PlayerUIHandler component not found on the GameObject.");
        }

        hitEffect = GetComponent<ParticleSystem>();

        if (hitEffect == null)
        {
           Debug.LogError("ParticleSystem component not found on the GameObject.");
        }
        // Initialize the last position to the current position
        lastPosition = transform.position;
        
        enemyExplode = GetComponent<EnemyExplode>();

        if (enemyExplode == null)
        {
           Debug.LogError("EnemyExplode component not found on the GameObject.");
        }

        rb = GetComponent<Rigidbody>();

       


    }

    private void Update()
    {
        bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            animator.SetFloat("Velocity", 0f);

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("Special animation key pressed.");
             
                // Get and play the player animator
                playerAnimator = player.GetComponent<Animator> ();
                playerAnimator.SetBool(SpecialParam, true);
                playerAnimator.SetInteger(specialIdHash, 21); // Right Kick

                TakeDamage(damage);
            }
            else 
            {
                AttackPlayer();
            }

        }
        else if (!playerInSightRange && takeDamage)
        {
            ChasePlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        animator.SetFloat("Velocity", 0.2f);

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

   private void ChasePlayer()
{
    navAgent.SetDestination(player.position);
    animator.SetFloat("Velocity", 0.6f);
    navAgent.isStopped = false; // Add this line
}




  private void AttackPlayer()
{
    navAgent.SetDestination(transform.position);

    if (!alreadyAttacked)
    {
        transform.LookAt(player.position);
        alreadyAttacked = true;
        animator.SetBool("Attack", true);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            //ShowPanel();
        }
        else
        {
            //HidePanel();
        }
  

    }
}


    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("Attack", false);
    }

    private void ShowPanel()
    {
            playerUIHandler.playerInside = true;
            // Show the warning panel
            playerUIHandler.ShowWarningPanel();
    }

    private void HidePanel()
    {
            playerUIHandler.playerInside = false;   
            // Hide the warning panel 
            playerUIHandler.HideWarningPanel();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (hitEffect != null)
        {
           hitEffect.Play();
        }
        
        StartCoroutine(TakeDamageCoroutine());

        if (health <= 0)
        {
            Debug.Log (gameObject.name +" health :"+ health);
            
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        takeDamage = true;
        yield return new WaitForSeconds(0.5f);
        takeDamage = false;
    }

    private void DestroyEnemy()
    {
        StartCoroutine(DestroyEnemyCoroutine());
    }

    private IEnumerator DestroyEnemyCoroutine()
    {
        animator.SetBool("Dead", true);

        // Freeze all position and rotation constraints
        rb.constraints = RigidbodyConstraints.FreezeAll;

        if (hitEffect != null)
        {
           hitEffect.Play();
        }

        Debug.Log("DestroyEnemyCoroutine Called");

        enemyExplode.EnemyExploder();

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
        
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    
    }
}
}