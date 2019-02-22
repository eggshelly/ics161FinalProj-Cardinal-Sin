using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMap : MonoBehaviour
{
    //public static Player instance;
    [SerializeField] protected float speed = 4f;
    private Rigidbody2D m_rigidbody2D;
    private Collider2D m_collider;
    private Vector2 moveVelocity;

    void Start()
    {
        m_rigidbody2D = this.GetComponent<Rigidbody2D>();
        m_collider = this.GetComponent<Collider2D>();   
    }
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
    }

    void FixedUpdate()
    {
        m_rigidbody2D.MovePosition(m_rigidbody2D.position + moveVelocity * Time.fixedDeltaTime);
        
    }
    void Move()
    {

    }
}
