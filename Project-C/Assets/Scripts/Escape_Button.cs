using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape_Button : MonoBehaviour
{
    public Escape_Door escapeDoor; // Reference to the Escape_Door script
    private bool isPlayerInRange = false;
    private bool isActivated = false;

    public float sinkDepth = 0.2f; // Depth to sink the button
    public float sinkSpeed = 2f; // Speed at which the button sinks

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            escapeDoor.RaiseDoor();
            StartCoroutine(SinkButton());
            isActivated = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private IEnumerator SinkButton()
    {
        Vector3 targetPosition = initialPosition - Vector3.back * sinkDepth;
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, sinkSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
