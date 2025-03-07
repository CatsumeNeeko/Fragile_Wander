using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float pullSpeed = 3f;
    [SerializeField] bool isBeingPulled = false;
    [SerializeField] bool returnToStart = false;
    private Vector3 playerPosition;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isBeingPulled)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            transform.position += direction * pullSpeed * Time.deltaTime;
        }
        else if (!isBeingPulled && returnToStart)
        {
            Vector3 direction = (startPosition - transform.position).normalized;
            transform.position += direction * pullSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, startPosition) < 0.1f)
            {
                transform.position = startPosition;
                //returnToStart = false;
            }
        }
    }

    public void StartPulling(Vector3 playerPos)
    {
        playerPosition = playerPos;
        isBeingPulled = true;
        //returnToStart = false;
    }

    public void StopPulling()
    {
        isBeingPulled = false;
    }

    public void SetReturnToStart(bool value)
    {
        returnToStart = value;
    }
}
