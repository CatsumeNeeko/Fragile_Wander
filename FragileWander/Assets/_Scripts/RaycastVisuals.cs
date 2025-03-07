using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastVisuals : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask enemyLayer;

    private LineRenderer lineRenderer;
    private Vector2 facingDirection = Vector2.right; // Default direction (e.g., facing right)

    void Start()
    {
        // Add and configure LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Update facing direction based on player movement
        if (Input.GetAxisRaw("Horizontal") > 0)
            facingDirection = Vector2.right;
        else if (Input.GetAxisRaw("Horizontal") < 0)
            facingDirection = Vector2.left;

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, rayDistance, enemyLayer);
        Vector2 endPoint = (Vector2)transform.position + (facingDirection * rayDistance);

        if (hit.collider != null)
        {
            endPoint = hit.point; // Stop at collision point
        }

        // Update LineRenderer positions
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
    }
}
