using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapMovement : MonoBehaviour
{
    [SerializeField] GameObject boundary; //A box collider used by the Cinemachine to confine the camera
    [SerializeField] float speed; //Player's map speed

    Rigidbody2D m_RigidBody2D;
    BoxCollider2D m_Collider;

    float left, right, top, bottom;

    private void Awake()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //Translates the player's position
    void Move()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");
        float yDirection = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3((CompletelyInBoundsX(xDirection) ? xDirection * speed: 0), (CompletelyInBoundsY(yDirection)? yDirection * speed: 0)) * Time.deltaTime);
    }


    //Makes sure the player does not leave the bounds of the map
    bool CompletelyInBoundsX(float xDir)
    {
        float l = 0;
        float r = 0;
        if (xDir < 0)
        {
            l = m_Collider.bounds.center.x - m_Collider.bounds.extents.x + xDir * speed * Time.deltaTime;
        }
        else if(xDir > 0)
        {
            r = m_Collider.bounds.center.x + m_Collider.bounds.extents.x + xDir * speed * Time.deltaTime;
        }
        return (l > left && r < right);
    }

    bool CompletelyInBoundsY(float yDir)
    {
        float b = 0;
        float t = 0;
        if (yDir < 0)
            b = m_Collider.bounds.center.y - m_Collider.bounds.extents.y;
        else if (yDir > 0)
        {
            t = m_Collider.bounds.center.y + m_Collider.bounds.extents.y;
        }
        return (t < top && b > bottom);
    }

    //Sets the boundaries based on the confining collider
    void SetBoundaries()
    {
        PolygonCollider2D bounds = boundary.GetComponent<PolygonCollider2D>();
        left = bounds.bounds.center.x - bounds.bounds.extents.x;
        right = bounds.bounds.center.x + bounds.bounds.extents.x;
        top = bounds.bounds.center.y + bounds.bounds.extents.y;
        bottom = bounds.bounds.center.y - bounds.bounds.extents.y;
    }


}
