using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, player.position, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
