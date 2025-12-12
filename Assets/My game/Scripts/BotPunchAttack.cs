using UnityEngine;

public class BotPunchAttack : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.2f;

    [Header("Animation Settings")]
    public Animator animator;
    public string punchTriggerName = "Punch";
    public float punchDuration = 0.6f;

    private float lastAttackTime;
    private AIWander wander;

    void Start()
    {
        wander = GetComponent<AIWander>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            TryPunch();
        }
    }

    void TryPunch()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        // Freeze movement
        if (wander != null)
            wander.isAttacking = true;

        // Play animation
        if (animator != null)
            animator.SetTrigger(punchTriggerName);

        // Damage player
        PlayerHealth hp = player.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(attackDamage);

        // Unfreeze after animation
        Invoke(nameof(EndPunch), punchDuration);
    }

    void EndPunch()
    {
        if (wander != null)
            wander.isAttacking = false;
    }
}