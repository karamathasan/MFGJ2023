using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    float HP = 100;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    public Sensor sensor;
    private bool m_grounded = false;

    private float hopTimer = 0;
    private float shorthopTimerThres = 0.1f;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        sensor = GetComponentInChildren<Sensor>();
    }

    // Update is called once per frame
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
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        //m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --

        //Hurt
        //else if (Input.GetKeyDown("q"))
        //    m_animator.SetTrigger("Hurt");

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
    }
    public void Jump()
    {
        m_animator.SetTrigger("Jump");
        m_grounded = false;
        m_animator.SetBool("Grounded", m_grounded);
        if (hopTimer > shorthopTimerThres)
        {
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        }
        else m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce / 2);

    }

}
