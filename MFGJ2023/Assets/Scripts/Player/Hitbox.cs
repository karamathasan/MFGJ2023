using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private LayerMask enemyLayer;
    public Vector2 hitboxLocation;
    public float radius;
    public float damage;

    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }
    //void Update()
    //{
     
    //}
    public void CheckHit()
    {
        hitboxLocation.x *= player.getDirection();
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(hitboxLocation + player.getPosition(), radius, enemyLayer);
        if (enemiesHit.Length == 0)
        {
            return;
        }
        foreach(var enemy in enemiesHit)
        {
            TestEnemy Enemy = enemy.GetComponentInParent<TestEnemy>();
            Enemy.TakeHit(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitboxLocation + player.getPosition(), radius);
    }
}
