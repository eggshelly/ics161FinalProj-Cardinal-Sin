using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int maxNumberOfJumps;
    [SerializeField] float jumpForce;


    Rigidbody2D m_RigidBody2D;
    CircleCollider2D m_CircleCollider2D;

    int jumpsRemaining;

    bool m_isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_CircleCollider2D = GetComponent<CircleCollider2D>();
        jumpsRemaining = maxNumberOfJumps;

    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();
        Move();
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }

    void Move()
    {
        m_RigidBody2D.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, m_RigidBody2D.velocity.y);
    }

    void Jump()
    {
        if(m_isGrounded)
        {
           m_RigidBody2D.velocity = new Vector2(m_RigidBody2D.velocity.x, 0);
            m_RigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsRemaining -= 1;
        }
        else if(!m_isGrounded && jumpsRemaining > 0)
        {
            m_RigidBody2D.velocity = new Vector2(m_RigidBody2D.velocity.x, 0);
            m_RigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsRemaining -= 1;
        }
    }

    void checkGrounded()
    {
        Vector3 pos = m_CircleCollider2D.bounds.center + m_CircleCollider2D.bounds.extents.y * Vector3.down;
        RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.down, 0.01f);
        if (ray.collider != null && ray.collider.CompareTag("Ground"))
        {
            m_isGrounded = true;
            jumpsRemaining = maxNumberOfJumps;
        }
        else
            m_isGrounded = false;   
    }


}
