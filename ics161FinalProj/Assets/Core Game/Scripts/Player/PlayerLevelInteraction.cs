using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelInteraction : MonoBehaviour
{
    [SerializeField] float MaxDistanceAway;

    float distanceAwayX;
    float distanceAwayY;

    Rigidbody2D ObjectToPull;
    Collider2D ObjectCollider;

    CapsuleCollider2D m_Collider;
    SpriteRenderer m_SpriteRenderer;
    PlayerLevelMovement m_LevelMovement;
    Rigidbody2D m_RigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<CapsuleCollider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_LevelMovement = GetComponent<PlayerLevelMovement>();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(ObjectToPull == null)
                m_LevelMovement.PullingObject(GrabItem());
            else
            {
                ObjectToPull = null;
                m_LevelMovement.PullingObject(false);
            }
        }
        MoveObject();
        
    }

    bool GrabItem()
    {
        Vector3 pos = m_Collider.bounds.center + (Vector3.down * m_Collider.bounds.extents.y / 2) + m_Collider.bounds.extents.x * (m_SpriteRenderer.flipX ? Vector3.left : Vector3.right);
        Debug.DrawLine(pos, pos + (m_SpriteRenderer.flipX ? Vector3.left : Vector3.right) * 0.5f);
        RaycastHit2D ray = Physics2D.Raycast(pos, (m_SpriteRenderer.flipX ? Vector2.left : Vector2.right), 0.5f);
        if(ray.collider!= null && ray.collider.CompareTag("Interactable"))
        {
            ObjectToPull = ray.collider.gameObject.GetComponent<Rigidbody2D>();
            ObjectCollider = ray.collider;
            distanceAwayY = Mathf.Abs(ObjectCollider.bounds.center.y - m_Collider.bounds.center.y);
            distanceAwayX = Mathf.Abs(ObjectCollider.bounds.center.x - m_Collider.bounds.center.x) + MaxDistanceAway;
            return true;
        }
        ObjectToPull = null;
        ObjectCollider = null;
        return false;
    }

    void MoveObject()
    {
        if(ObjectToPull != null)
        {
            if(Mathf.Abs(ObjectCollider.bounds.center.x - m_Collider.bounds.center.x) > distanceAwayX || Mathf.Abs(ObjectCollider.bounds.center.y - m_Collider.bounds.center.y) > distanceAwayY)
            {
                ObjectToPull = null;
                ObjectCollider = null;
                m_LevelMovement.PullingObject(false);
            }
            else
                ObjectToPull.velocity = new Vector3(m_RigidBody.velocity.x, 0,0);
        }
    }
}
