using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false; //! Deprecated, timerınkiyle değiştir 

    [SerializeField] protected float speed;

    [SerializeField] protected float damage;

    protected Timer recoilTimer = new();
    protected Rigidbody2D rb;





    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        recoilTimer.duration = recoilLength;
    }
    protected virtual void Update()
    {
        if (IsDead())
        {
            Destroy(gameObject);
            //TODO add death animation, manage with event, make HandleDeath()
        }

        HandleRecoil();
    }

    bool IsDead() => health <= 0;

    void HandleRecoil()
    {
        if (!isRecoiling) return;
        recoilTimer.Tick();
        if (!recoilTimer.IsFinished()) return;
        recoilTimer.Reset();
        isRecoiling = false;
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
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
        PlayerController.Instance.TakeDamage(damage);
    }
}
