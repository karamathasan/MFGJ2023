using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 9.5f;
    float HP = 100;

    private float hopTimer = 0;
    private float shorthopTimerThres = 0.1f;
    private bool m_grounded = false;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    [SerializeField]
    Sensor sensor;

    public bool dodgeAcquired;
    public bool canDodge = true;
    public bool dashAcquired;
    private bool canDash = true;
    public bool counterAcquired;
    private bool canCounter;

    public bool invincibility = false;

    // Use this for initialization
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        sensor = GetComponentInChildren<Sensor>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_grounded)
        {
            m_body2d.AddForce(new Vector2(0, -6));
        }
    }
    void Update()
    {
        //Check if character just landed on the ground
        if (!m_grounded && sensor.Grounded())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !sensor.Grounded())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }


        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX < 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX > 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        Walk();
        //m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        //m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        //Attack
        if (Input.GetKeyDown("j"))
        {
            m_animator.SetTrigger("Attack");
        }

        else if (Input.GetKey("space") && m_grounded)
        {
            if (hopTimer >= shorthopTimerThres)
            {
                Jump();
                hopTimer = 0;
            }
            hopTimer += Time.deltaTime;
        }
        else if (Input.GetKeyUp("space") && m_grounded)
        {
            Jump();
            hopTimer = 0;
        }
        else if (dodgeAcquired && canDodge && Input.GetKeyDown("l"))
        {
            //invincibility = true;
            StartCoroutine(Dodge());
        }
        else if (counterAcquired && canCounter && Input.GetKeyDown("h"))
        {
            //invincibility = true;
            //StartCoroutine(Dodge());
        }
        else if (dashAcquired && canDash && (Input.GetKeyDown("l") && Mathf.Abs(m_body2d.velocity.x) > 1))
        {
            //invincibility = true;
            //StartCoroutine(Dodge());
        }
    }
    private void Walk()
    {
        //m_body2d.velocity = new Vector2(Input.GetAxis("Horizontal") * m_speed, m_body2d.velocity.y);

        float targetSpeed = m_speed * (Input.GetAxisRaw("Horizontal"));
        float error = targetSpeed - m_body2d.velocity.x;
        float propTerm = error * 1f;
        m_body2d.AddForce(propTerm * Vector2.right, ForceMode2D.Force);
    }
    /// 1 dodge
    /// 2 dash
    ///  block
    ///  counter
    private IEnumerator Dodge()
    {
        invincibility = true;
        canDodge = false;
        m_animator.SetInteger("ActionID", 1);
        yield return new WaitForSeconds(0.5f);
        invincibility = false;
        m_animator.SetInteger("ActionID", 0);
        yield return new WaitForSeconds(1f);
        canDodge = true;

    }
    public void Jump()
    {
        m_grounded = false;
        m_animator.SetBool("Grounded", m_grounded);
        if (hopTimer > shorthopTimerThres)
        {
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        }
        else m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce / 1.4f);
    }

}
