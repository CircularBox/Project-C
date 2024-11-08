using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    Animator animator;
    public float attackDistance = 1.5f; // Adjust this value based on your attack range
    public int damageAmount = 10;
    float attackTimer = 0f; // Timer to track attack cooldown
    public float attackCooldown = 1f; // Time between attacks (adjust based on animation)
    Field_Of_View fieldOfView; // Reference to the Field_Of_View component
    AI_Wandering aiWandering; // Reference to the AI_Wandering component

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<Field_Of_View>(); // Get the Field_Of_View component
        aiWandering = GetComponent<AI_Wandering>(); // Get the AI_Wandering component

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ensure fieldOfView is assigned before setting the target
        if (fieldOfView != null)
        {
            target = fieldOfView.playerRef != null ? fieldOfView.playerRef.transform : null;

            bool canSeePlayer = fieldOfView.canSeePlayer;
            bool isPlayerInMemory = fieldOfView.memoryTimer > 0;

            if (target != null && (canSeePlayer || isPlayerInMemory))
            {
                navMeshAgent.isStopped = false;
                aiWandering.enabled = false; // Disable wandering behavior

                navMeshAgent.SetDestination(target.position);

                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                // Attack when in range
                if (distanceToPlayer <= attackDistance && attackTimer <= 0f)
                {
                    attackTimer = attackCooldown;
                    navMeshAgent.isStopped = true;
                    animator.SetTrigger("Attack");
                    AttackPlayer(target.gameObject);
                }

                if (attackTimer > 0f) // Decrement timer if attack is on cooldown
                {
                    attackTimer -= Time.deltaTime;
                }

                // Check if animation finished and attack timer depleted
                if (!animator.IsInTransition(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && attackTimer <= 0f)
                {
                    // Resume agent movement
                    navMeshAgent.isStopped = false;
                }
            }
            else
            {
                navMeshAgent.isStopped = true; // Stop the agent if the player is not visible and not in memory
                aiWandering.enabled = true; // Enable wandering behavior
            }
        }
    }

    public void AttackPlayer(GameObject player) // Define the method here
    {
        // Get the player health script
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Deal damage to the player
            playerHealth.TakeDamage(damageAmount);
        }
    }
}