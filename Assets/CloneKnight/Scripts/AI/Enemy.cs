using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected float speed;

    [SerializeField] protected float damage;

    protected Timer recoilTimer = new();
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isDead = false;
    PlayerCombat playerCombat;



    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        recoilTimer.duration = recoilLength;
        playerCombat = PlayerCombat.Instance;
    }

    protected virtual void Update()
    {
        if (IsDead() && !isDead)
        {
            HandleDeath();

        }

        HandleRecoil();
    }

    protected bool IsDead() => health <= 0;

    void HandleRecoil()
    {


        isRecoiling = false;
    }

    protected virtual void HandleDeath()
    {
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        Destroy(gameObject, 1.0f); // Ölüm animasyonu için gecikmeli silme
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        if (isDead) return;

        health -= _damageDone;
        if (isRecoiling) return;

        rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
        isRecoiling = true;
    }

    protected void OnCollisionStay2D(Collision2D _other)
    {
        if (!_other.gameObject.CompareTag("Player") || PlayerController.Instance.pState.invincible) return;
        Attack();
        PlayerController.Instance.HitStopTime(0, 5, 0.5f);
    }

    protected virtual void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        playerCombat.TakeDamage(damage);
    }
}