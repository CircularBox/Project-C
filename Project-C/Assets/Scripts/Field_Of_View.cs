using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field_Of_View : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool canSeePlayer;
    public float memoryDuration = 3f; // Duration for which the enemy remembers the player
    public float memoryTimer; // Timer to remember the player

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine() // Should help performance by limiting search frequency
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true) // Infinite loop
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in rangeChecks)
        {
            if (collider.CompareTag("Player")) // Ensure the target is tagged as "Player"
            {
                Transform target = collider.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                    {
                        if (distanceToTarget < closestDistance)
                        {
                            closestDistance = distanceToTarget;
                            closestPlayer = target.gameObject;
                        }
                    }
                }
            }
        }

        if (closestPlayer != null)
        {
            playerRef = closestPlayer;
            canSeePlayer = true;
            memoryTimer = memoryDuration; // Reset the memory timer
        }
        else
        {
            // If the player is not seen, decrement the memory timer
            if (memoryTimer > 0)
            {
                memoryTimer -= Time.deltaTime;
            }
            else
            {
                canSeePlayer = false;
                playerRef = null;
            }
        }
    }
}
