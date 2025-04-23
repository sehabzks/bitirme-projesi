using UnityEngine;

public class Zombie : Enemy
{
    Transform playerTransform;
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        playerTransform = PlayerController.Instance.transform;
    }

    protected override void Update()
    {
        base.Update();
        if (!isRecoiling)
        {
            transform.position = Vector2.MoveTowards
                (transform.position,
                new Vector2(playerTransform.position.x, transform.position.y),
                speed * Time.deltaTime);
        }
    }
    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);
    }

}
