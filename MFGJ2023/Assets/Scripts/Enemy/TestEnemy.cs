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
    [SerializeField]
    private float hp = 50;
    [SerializeField]
    private float aggression = 1;
    private float maxStamina = 10;
    [SerializeField]
    private float stamina;

    private bool retreating = false;

    private bool detectedPlayer = false;

    public Player player;

    public enum States
    {
        approaching = 0,
        attack = 1,
        distancing = 2
    }
    [SerializeField]
    private int currentState = 0;

    // Use this for initialization
    void Start()
    {
        stamina = maxStamina;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        //m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        m_animator.SetFloat("AnimState", 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStamina();
        FacePlayer();

        if (!detectedPlayer)
        {
            if (DistToPlayer() < 7)
            {
                m_animator.SetInteger("AnimState", 1);
                detectedPlayer = true;
            }
            return;
        }
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
            case -1://the empty state, use this if you need to "pause" the state machine
                break;
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
    /*--------------
     * State Methods
     --------------*/

    private void Approach()
    {
        retreating = false;
        if(aggression == 0)
        {
            currentState = (int)States.distancing;
        }
        if(DistToPlayer() < 1.5)
        {
            currentState++;
        }
        m_animator.SetInteger("AnimState", 2);
        m_body2d.velocity = new Vector2(-DirectionToPlayer() * m_speed, m_body2d.velocity.y);
    }
    private void Attack()
    {
        m_animator.SetTrigger("Attack");
        currentState = (int)States.attack;
        if (stamina - 3 > 0)
        {
            stamina -= 3;
        }
        currentState = -1;//the attack animation puts the enemy back into the attacking state to avoid never completing the animation
    }
    public void EndAttack()
    {
        float randomNum = Random.Range(0f, 1f);
        if (randomNum > stamina / maxStamina)
        {
            currentState = (int)States.distancing;
        }
        else currentState = (int)States.approaching;
    }
    private void Distance()
    {
        if (DistToPlayer() <= 3)
        {
            retreating = true;
            if (m_body2d.velocity.x < 0)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

            m_animator.SetInteger("AnimState", 2);
            m_body2d.velocity = new Vector2(DirectionToPlayer() * m_speed, m_body2d.velocity.y);
        }
        else if (DistToPlayer() >= 6) {
            retreating = false;
            m_animator.SetInteger("AnimState", 2);
            m_body2d.velocity = new Vector2(-DirectionToPlayer() * m_speed, m_body2d.velocity.y);
        }
        if (Mathf.Abs(m_body2d.velocity.x) < 0.7)
        {
            retreating = false;
            m_animator.SetInteger("AnimState", 1);
        }
        if (2 * Random.Range(0f,100f) < 20 * aggression && stamina/maxStamina > 0.8f)
        {
            currentState = (int)States.approaching;
        }
    }
    /*--------
    * Utiltity  
    ---------*/
    private float DistToPlayer()
    {
        return Mathf.Abs(m_body2d.position.x - player.transform.position.x);
    }
    private float DirectionToPlayer()
    {
        return Mathf.Sign(m_body2d.position.x - player.transform.position.x);
    }

    /*------------------
     * Frame-Based methods
     -------------------*/

    private void FacePlayer()
    {
        if (retreating)
        {
            return;
        }
        if (DirectionToPlayer() < 0)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }else transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    private void UpdateStamina()
    {
        if (stamina < maxStamina)
        {
            stamina += Time.deltaTime;
        }

    }
    public void TakeHit(float damage)
    {
        m_animator.SetTrigger("Hurt");
        hp -= damage;
        currentState = (int)States.distancing;
        Die();
    }
    private void Die()
    {
        if (hp <= 0)
        {
            //m_animator.SetTrigger("Hurt");
            m_animator.SetTrigger("Death");
            currentState = -1;
            Destroy(this);
        }
    }
}

