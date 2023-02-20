using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public float m_speed = 5;
  public float m_force = 10;
  public float m_punchForce = 1000;
  public float m_stickThreshold = .125f;
  bool m_hasAction = false;
  bool m_isGrounded = true;
  bool m_isPunching = false;
  Vector2 m_direction = Vector2.zero;
  public GameObject[] m_punchAreaObjs;
  public List<BoxCollider2D> m_punchAreas;

  Rigidbody2D m_rb;

  // Start is called before the first frame update
  void Awake()
  {
    m_rb = GetComponent<Rigidbody2D>();
    //m_punchAreaObjs = GameObject.FindGameObjectsWithTag("Punch");
    for (int i = 0; i < m_punchAreaObjs.Length; i++)
    {
      m_punchAreas.Add(m_punchAreaObjs[i].GetComponent<BoxCollider2D>());
    }
  }

  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    // GROUNDED RAYCASTING
    int m_elementLayerMask = 1 << 3;
    float m_raySize = 1.25f;
    RaycastHit2D hit;

    hit = Physics2D.Raycast(transform.position, Vector3.down, m_raySize, m_elementLayerMask);

    if (hit)
    {
      Debug.DrawRay(transform.position, Vector3.down * m_raySize, Color.blue);
      m_isGrounded = true;
    }
    else
    {
      Debug.DrawRay(transform.position, Vector3.down * m_raySize, Color.red);
      m_isGrounded = false;
    }

    if (InputManager.Instance.ActionExecuted())
    {
      m_hasAction = true;
    }
    if (InputManager.Instance.ActionCanceled() && m_hasAction)
    {
      m_hasAction = false;
    }
    if (m_hasAction && InputManager.Instance.MoveDirection().magnitude >= m_stickThreshold)
    {
      m_direction = InputManager.Instance.MoveDirection();
      Debug.Log(m_direction);
    }
    if (m_hasAction && InputManager.Instance.MoveDirection().magnitude <= m_stickThreshold && m_direction != Vector2.zero && m_isGrounded)
    {
      Debug.Log("Added force to " + m_direction);
      m_rb.AddForce(m_direction.normalized * -1 * m_force);
      m_direction = Vector2.zero;
    }
  }

  private void FixedUpdate()
  {
    if (InputManager.Instance.MoveDirection() != Vector2.zero && !m_hasAction)
    {
      transform.position += new Vector3(InputManager.Instance.MoveDirection().x, 0, 0) * m_speed;
    }

    if (InputManager.Instance.ActionExecuted())
    {
      Debug.Log("Punched Enemy");
      m_isPunching = true;
    }
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    if(collision.CompareTag("Enemy"))
    {
      if(m_isPunching)
      {
        collision.attachedRigidbody.AddForce((collision.transform.position + (Vector3.up / 2) - transform.position).normalized * m_punchForce);
        m_isPunching = false;
      }
    }
  }
}