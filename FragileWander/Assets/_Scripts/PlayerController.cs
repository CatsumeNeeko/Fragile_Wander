using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rayDistance = 10f;
    [SerializeField] List<LayerMask> enemyLayer;
    [SerializeField] GameObject sceneManager;
    private Vector2 moveDirection;
    private Vector2 facingDirection = Vector2.up;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private GameObject currentEnemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        HandleMovement();
        HandleFacingDirection();
        ShootRaycast();
        VisualizeRaycast();
    }

    void HandleMovement()
    {
        moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) moveDirection.y = 1;
        if (Input.GetKey(KeyCode.S)) moveDirection.y = -1;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = 1;
        rb.velocity = moveDirection.normalized * moveSpeed;
    }

    void HandleFacingDirection()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) SetFacingDirection(Vector2.up, 0f);
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetFacingDirection(Vector2.down, 180f);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SetFacingDirection(Vector2.left, 90f);
        if (Input.GetKeyDown(KeyCode.RightArrow)) SetFacingDirection(Vector2.right, -90f);
    }
    void SetFacingDirection(Vector2 direction, float rotationZ)
    {
        facingDirection = direction;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    void ShootRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, rayDistance, enemyLayer[0]);
        Debug.DrawRay(transform.position, facingDirection * rayDistance, Color.red);
        if (hit.collider != null)
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.StartPulling(transform.position);
            }
        }
        else
        {
            EnemyController[] allEnemies = FindObjectsOfType<EnemyController>();
            foreach (var enemy in allEnemies)
            {
                enemy.StopPulling();
            }
        }
    }
    void VisualizeRaycast()
    {
        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, rayDistance, layerMask);
        Debug.Log(hit.collider.gameObject.name);
        Vector2 endPoint = (Vector2)transform.position + (facingDirection * rayDistance);

        // If the ray hits something with a valid tag, shorten it
        if (hit.collider != null && (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Goal") || hit.collider.CompareTag("Object")))
        {
            endPoint = hit.point; // Stop at the collision point
        }
        else { }

        // Update LineRenderer positions
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            sceneManager.GetComponent<SceneManagment>().ReloadLevel();
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            sceneManager.GetComponent<SceneManagment>().LoadNewLevel();
        }
    }
}
