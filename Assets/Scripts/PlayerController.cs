using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Capturamos input (no física acá)
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        movementInput = movementInput.normalized;
        
        HandleShooting();
    }

    private void FixedUpdate()
    {
        // Movimiento físico
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void HandleShooting()
    {
        Vector2 shootDirection = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            shootDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            shootDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            shootDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            shootDirection = Vector2.right;

        if (shootDirection != Vector2.zero)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetDirection(shootDirection);
        }
    }
}