using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelInteraction : MonoBehaviour
{
    [SerializeField] float MaxDistanceAway;

    float distanceAwayX;
    float distanceAwayY;

    GameObject ObjectToPull;

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
                DetachObject();
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
            ObjectToPull = ray.collider.gameObject;
            float distanceAway = ray.collider.bounds.center.x - m_Collider.bounds.center.x;
            ObjectToPull.transform.parent = transform;
            return true;
        }
        ObjectToPull = null;
        return false;
    }

    void MoveObject()
    {
        if(ObjectToPull != null)
        {
            ObjectToPull.transform.Translate(m_RigidBody.velocity * Time.deltaTime);
        }
    }

    public void DetachObject()
    {
        transform.DetachChildren();
        ObjectToPull = null;
    }
}
