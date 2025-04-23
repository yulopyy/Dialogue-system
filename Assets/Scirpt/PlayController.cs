using UnityEngine;

public class PlayerMovementSimple : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;   // <<<<<<<<<<<< เปลี่ยนเป็น Rigidbody2D

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // <<<<<<<<<<<< เปลี่ยนเป็น Rigidbody2D
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");  // <<<<<<<<<<<< ใช้ y ไม่ใช่ z
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
