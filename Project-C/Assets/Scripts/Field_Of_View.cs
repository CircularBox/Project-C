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
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine() //Should help performance by limiting search frequency
    {
        float delay = 0.1f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true) //infinite loop
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    canSeePlayer = true;
                    memoryTimer = memoryDuration; // Reset the memory timer
                    return; // Exit the method early if the player is seen
                }
            }
        }

        // If the player is not seen, decrement the memory timer
        if (memoryTimer > 0)
        {
            memoryTimer -= Time.deltaTime;
        }
        else
        {
            canSeePlayer = false;
        }
    }
}
