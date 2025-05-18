using System.Collections;
using UnityEngine;

public class PlayerMovement : PersistentSingleton<PlayerMovement>
{
    PlayerStateList pState;
    PlayerData playerData;

    protected override void Awake()
    {
        base.Awake();
        pState = GetComponent<PlayerStateList>();
    }

    void Start()
    {
        playerData = PlayerData.Instance;
    }

    void Update()
    {
        UpdateJumpVariables();
        
        // Dashing durumunda Move() fonksiyonunu çağırmayı engelliyoruz
        if (!pState.dashing)
        {
            Move();
        }
        
        Flip();
        Jump();
        StartDash();
    }

    private void Move()
    {
        if (pState.healing) playerData.SetLinearVelocity(new Vector2(0, 0)); //! Buraya bi return gerekiyor olabilir, test edilmeli
        playerData.SetLinearVelocity(new Vector2(playerData.WalkSpeed * playerData.xAxis, playerData.GetLinearVelocity().y));
        playerData.SetAnimBool("Walking", playerData.GetLinearVelocity().x != 0 && IsGrounded());
    }

    void Flip()
    {
        if (playerData.xAxis == 0) return;
        pState.lookingRight = playerData.xAxis > 0;
        var direction = pState.lookingRight ? 1 : -1;

        transform.localScale = new Vector2(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && playerData.canDash && !playerData.dashed)
        {
            StartCoroutine(Dash());
            playerData.dashed = true;
        }

        if (IsGrounded())
        {
            playerData.dashed = false;
        }
    }

    IEnumerator Dash()
    {
        pState.invincible = true;
        playerData.canDash = false;
        pState.dashing = true;
        playerData.SetAnimTrigger("Dashing");
        playerData.SetGravityScale(0);
        
        int _dir = pState.lookingRight ? 1 : -1;
        float dashStartTime = Time.time;
        
        // Dash effect
        if (IsGrounded() && playerData.dashEffect != null)
        {
            playerData.dashEffect = Instantiate(playerData.dashEffect, transform);
        }
        
        // Dash süresi boyunca velocity'i sürekli güncelliyoruz
        while (Time.time < dashStartTime + playerData.DashTime)
        {
            playerData.SetLinearVelocity(new Vector2(_dir * playerData.DashSpeed, 0));
            yield return null;
        }
        
        playerData.SetGravityScale(playerData.Gravity);
        pState.dashing = false;
        pState.invincible = false;
        
        yield return new WaitForSeconds(playerData.DashCooldown);
        playerData.canDash = true;
    }

    public bool IsGrounded()
    {
        Vector2 origin = playerData.groundCheckPoint.position;
        Vector2 offset = new(playerData.GroundCheckX, 0);
        float distance = playerData.GroundCheckY;

        return
        Physics2D.Raycast(origin, Vector2.down, distance, playerData.whatIsGround) ||
        Physics2D.Raycast(origin + offset, Vector2.down, distance, playerData.whatIsGround) ||
        Physics2D.Raycast(origin - offset, Vector2.down, distance, playerData.whatIsGround);
    }

    void Jump()
    {
        bool canGroundedJump = playerData.JumpBufferCounter > 0 && playerData.CoyoteTimeCounter > 0 && !pState.jumping;
        bool canDoubleJump = !IsGrounded() && playerData.AirJumpCounter < playerData.MaxAirJumps && Input.GetButtonDown("Jump");

        if (canGroundedJump)
        {
            PerformJump();
            pState.jumping = true;
        }

        if (canDoubleJump)
        {
            pState.jumping = true;
            playerData.AirJumpCounter++;
            PerformJump();
        }

        //Early jump cancel
        if (Input.GetButtonUp("Jump") && playerData.GetLinearVelocity().y > 3)
        {
            CancelJump();
        }

        playerData.SetAnimBool("Jumping", !IsGrounded());
    }
    void PerformJump()
    {
        playerData.SetLinearVelocity(new Vector3(playerData.GetLinearVelocity().x, playerData.JumpForce));
    }
    void CancelJump()
    {
        pState.jumping = false;
        playerData.SetLinearVelocity(new Vector2(playerData.GetLinearVelocity().x, 0));
    }
    void UpdateJumpVariables()
    {
        if (IsGrounded())
        {
            pState.jumping = false;
            playerData.CoyoteTimeCounter = playerData.CoyoteTime;
            playerData.AirJumpCounter = 0;
            // Clean the buffer
            playerData.JumpBufferCounter = 0;
        }
        else
        {
            playerData.CoyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            playerData.JumpBufferCounter = playerData.JumpBufferFrames;
        }
        else
        {
            playerData.JumpBufferCounter -= Time.deltaTime * 10;
        }
    }

    public IEnumerator WalkIntoNewScene(Vector2 _exitDir, float _delay) //
    {
        pState.invincible = true;

        //If exit direction is upwards
        if (_exitDir.y != 0)
        {
            playerData.SetLinearVelocity(playerData.JumpForce * _exitDir);
        }

        //If exit direction requires horizontal movement
        if (_exitDir.x != 0)
        {
            playerData.xAxis = _exitDir.x > 0 ? 1 : -1;

            Move();
        }

        Flip();
        yield return new WaitForSeconds(_delay);
        LogSystem.Log("Cutscene played");
        pState.invincible = false;
        pState.cutscene = false;
    }

}
