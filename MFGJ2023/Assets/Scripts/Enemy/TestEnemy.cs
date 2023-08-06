using UnityEngine;
using System.Collections;

public class TestEnemy : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    //private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    public Player player;

    public enum States
    {
        approaching = 0,
        attack = 1,
        distancing = 2
    }
    public int currentState = 0;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        //m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if character just landed on the ground
        //if (!m_grounded && m_groundSensor.State())
        //{
        //    m_grounded = true;
        //    m_animator.SetBool("Grounded", m_grounded);
        //}

        ////Check if character just started falling
        //if (m_grounded && !m_groundSensor.State())
        //{
        //    m_grounded = false;
        //    m_animator.SetBool("Grounded", m_grounded);
        //}

        // -- Handle input and movement --
        

        switch (currentState)
        {
            case 0:
                Approach();
                break;
            case 1:
                Attack();
                break;
            case 2:
                Distance();
                break;
        }        

    }
    private void Approach()
    {
        if(DistToPlayer() < 1.5)
        {
            currentState++;
        }
        m_body2d.velocity = new Vector2(-DirectionToPlayer() * m_speed, m_body2d.velocity.y);
    }
    private void Attack()
    {
        Debug.Log("attacking");
        currentState++;
    }
    private void Distance()
    {
        if (DistToPlayer() <= 3)
        {
            m_body2d.velocity = new Vector2(DirectionToPlayer() * m_speed, m_body2d.velocity.y);
        }
        else if (DistToPlayer() >= 6) {
            m_body2d.velocity = new Vector2(-DirectionToPlayer() * m_speed, m_body2d.velocity.y);
        }
    }
    private float DistToPlayer()
    {
        return Mathf.Abs(m_body2d.position.x - player.transform.position.x);
    }
    private float DirectionToPlayer()
    {
        return Mathf.Sign(m_body2d.position.x - player.transform.position.x);
    }
}

