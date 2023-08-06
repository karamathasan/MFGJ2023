using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public LayerMask ground;
    //public float timer = 0;

    public bool Grounded()
    {
        return Physics2D.OverlapCircle(gameObject.transform.position, 0.01f, ground);
    }

    void Update()
    {
        //if (timer < 0)
        //{
        //    timer = 0;
        //}
        //timer -= Time.deltaTime;
    }


}
