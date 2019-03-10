using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float startAttack;
    public Transform attackPosition;
    public float attackRange;
    public LayerMask IsEnimies;
    public int damage;
    public Animator animator;

    private float timeBetweenAttack;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (timeBetweenAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetTrigger("Attack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, IsEnimies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
                timeBetweenAttack = startAttack;
                
                
            }
        }
        else
        {
            timeBetweenAttack -= Time.deltaTime;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}

