using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Wandering : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotSpeed = 100f;

    private bool isWandering = false;
    private bool isRotating = false;
    private bool isWalking = false;
    private Quaternion targetRotation;

    // Update is called once per frame
    void Update()
    {
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isRotating == true)
        {
            float step = rotSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isRotating = false;
            }
        }
        if (isWalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator Wander()
    {
        int rotateWait = Random.Range(1, 4);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 3);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);

        // Generate a random angle between -180 and 180 degrees
        float randomAngle = Random.Range(-180f, 180f);
        targetRotation = Quaternion.Euler(0, randomAngle, 0) * transform.rotation;
        isRotating = true;

        // Wait until the rotation is complete
        yield return new WaitUntil(() => !isRotating);

        isWandering = false;
    }
}
