using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] bool canRun;
    [SerializeField] int maxNumberOfJumps;
    [SerializeField] float jumpForce;

    float speed;

    Rigidbody2D m_RigidBody2D;
    CapsuleCollider2D m_CapsuleCollider2D;
    SpriteRenderer m_SpriteRenderer;

    LevelManager manager;

    int jumpsRemaining;

    bool m_isGrounded;

    bool canExit = false;
    // Start is called before the first frame update
    void Start()
    {
        speed = walkSpeed;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelManager>();
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        jumpsRemaining = maxNumberOfJumps;

    }

    // Update is called once per frame
    void Update()
    {
        CheckExit();

        checkGrounded();
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    //Sets player's velocity and flips the character depending on what direction it is moving 
    void Move()
    {
        float movementModifier = Input.GetAxisRaw("Horizontal");
        if(Input.GetKey(KeyCode.LeftShift) && canRun)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        if (movementModifier > 0)
        {
            m_SpriteRenderer.flipX = false;
        }
        else if (movementModifier < 0)
        {
            m_SpriteRenderer.flipX = true;
        }

        m_RigidBody2D.velocity = new Vector3(movementModifier * speed, m_RigidBody2D.velocity.y);
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
        Vector3 pos = m_CapsuleCollider2D.bounds.center + m_CapsuleCollider2D.bounds.extents.y * Vector3.down;
        RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.down, 0.01f);
        if (ray.collider != null && ray.collider.CompareTag("Ground"))
        {
            m_isGrounded = true;
            jumpsRemaining = maxNumberOfJumps;
        }
        else
            m_isGrounded = false;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Exit"))
        {
            canExit = collision.gameObject.GetComponent<ExitDoorScript>().canExit;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stage"))
        {
            canExit = false;
        }
    }


    //If the player can exit (collected all collectibles), if they press E in front of the door, then leave the level
    void CheckExit()
    {
        if(canExit)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                manager.PassDataToSaveManager();

            }
        }

    }
}
