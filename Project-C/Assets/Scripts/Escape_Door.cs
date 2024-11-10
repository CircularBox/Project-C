using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape_Door : MonoBehaviour
{
    public float raiseHeight = 5f; // Height to raise the door
    public float raiseSpeed = 2f; // Speed at which the door raises

    private Vector3 initialPosition;
    private bool isRaising = false;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRaising)
        {
            float step = raiseSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + Vector3.up * raiseHeight, step);

            if (transform.position == initialPosition + Vector3.up * raiseHeight)
            {
                isRaising = false;
            }
        }
    }

    public void RaiseDoor()
    {
        isRaising = true;
    }
}
