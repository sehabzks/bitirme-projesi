using UnityEngine;

public class Zombie : Enemy
{
    Transform playerTransform;
    PlayerController playerController;
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        playerTransform = playerController.transform;
    }

    protected override void Update()
    {
        LogSystem.LogError(transform.name + " " + transform.position + " " + playerTransform.name + " " + playerTransform.position);
        base.Update();
        if (!isRecoiling)
        {
            transform.position = Vector2.MoveTowards
                (transform.position,
                new Vector2(playerTransform.position.x, transform.position.y),
                speed * Time.deltaTime);
        }
    }
    public override void TakeHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.TakeHit(_damageDone, _hitDirection, _hitForce);
    }

}
