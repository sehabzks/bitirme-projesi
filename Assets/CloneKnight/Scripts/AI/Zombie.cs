using UnityEngine;

public class Zombie : Enemy
{
    Transform playerTransform;
    Animator anim;

    Vector2 leftPoint;
    Vector2 rightPoint;
    bool movingRight = true;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float forgetPlayerDistance = 8f; // Takibi bırakma mesafesi

    private bool isChasing = false;

    PlayerController playerController;
    protected override void Start()
    {
        base.Start();
        playerController = PlayerController.Instance;
        rb.gravityScale = 12f;
        playerTransform = PlayerController.Instance.transform;
        anim = GetComponent<Animator>();

        if (pointA != null && pointB != null)
        {
            leftPoint = pointA.position;
            rightPoint = pointB.position;
        }
        else
        {
            Debug.LogWarning("Zombie patrol points not set!");
        }

    }

    protected override void Update()
    {
        base.Update();
        if (isRecoiling || IsDead()) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Takip moduna gir
        if (!isChasing && distanceToPlayer <= chaseRange)
        {
            isChasing = true;
        }

        // Takip modunu bırak (istersen)
        if (isChasing && distanceToPlayer > forgetPlayerDistance)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer(float distanceToPlayer)
    {
        Vector2 targetPos = new Vector2(playerTransform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        FlipDirection(playerTransform.position.x);

        if (distanceToPlayer <= attackRange)
        {
            anim.SetBool("IsWalking", false);
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("IsWalking", true);
            anim.SetBool("Attack", false);
        }
    }

    void Patrol()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("IsWalking", true);

        if (movingRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, rightPoint, speed * Time.deltaTime);
            FlipDirection(rightPoint.x);

            if (Vector2.Distance(transform.position, rightPoint) < 0.1f)
                movingRight = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, leftPoint, speed * Time.deltaTime);
            FlipDirection(leftPoint.x);

            if (Vector2.Distance(transform.position, leftPoint) < 0.1f)
                movingRight = true;
        }
    }

    void FlipDirection(float targetX)
    {
        Vector3 scale = transform.localScale;
        if (targetX > transform.position.x)
            scale.x = Mathf.Abs(scale.x); // sağa bak
        else
            scale.x = -Mathf.Abs(scale.x); // sola bak
        transform.localScale = scale;
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.TakeHit(_damageDone, _hitDirection, _hitForce);
    }
}
