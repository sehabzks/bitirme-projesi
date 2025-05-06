using UnityEngine;

public class PlayerData : PersistentSingleton<PlayerData>
{
    #region Horizontal Movement Variables
    [Header("Horizontal Movement Settings:")]
    [SerializeField] float walkSpeed = 1; //sets the players movement speed on the ground
    [SerializeField] float minWalkSpeed = 0.5f; //sets the minimum speed the player can move at
    [SerializeField] float maxWalkSpeed = 2f; //sets the maximum speed the player can move at
    public float WalkSpeed
    {
        get { return walkSpeed; }
        set
        {
            walkSpeed = Mathf.Clamp(value, minWalkSpeed, maxWalkSpeed);
        }
    }
    #endregion

    #region Vertical Movement Variables
    [Space(10), Header("Vertical Movement Settings")]
    [SerializeField] float jumpForce = 45f; //sets how high the player can jump
    public float JumpForce //allows other classes to access the jump force
    {
        get { return jumpForce; }
        set
        {
            jumpForce = value;
        }
    }

    float jumpBufferCounter = 0; //stores the jump button input
    public float JumpBufferCounter //allows other classes to access the jump buffer counter
    {
        get { return jumpBufferCounter; }
        set
        {
            jumpBufferCounter = value;
        }
    }
    [SerializeField] float jumpBufferFrames; //sets the max amount of frames the jump buffer input is stored
    public float JumpBufferFrames //allows other classes to access the jump buffer frames
    {
        get { return jumpBufferFrames; }
    }

    float coyoteTimeCounter = 0; //stores the Grounded() bool
    public float CoyoteTimeCounter //allows other classes to access the coyote time counter
    {
        get { return coyoteTimeCounter; }
        set
        {
            coyoteTimeCounter = value;
        }
    }
    [SerializeField] float coyoteTime; //sets the max amount of frames the Grounded() bool is stored
    public float CoyoteTime //allows other classes to access the coyote time
    {
        get { return coyoteTime; }
    }

    int airJumpCounter = 0; //keeps track of how many times the player has jumped in the air
    public int AirJumpCounter //allows other classes to access the air jump counter
    {
        get { return airJumpCounter; }
        set
        {
            airJumpCounter = value;
        }
    }
    [SerializeField] private int maxAirJumps; //the max no. of air jumps
    public int MaxAirJumps //allows other classes to access the max air jumps
    {
        get { return maxAirJumps; }
    }

    float gravity; //stores the gravity scale at start
    public float Gravity //allows other classes to access the gravity scale
    {
        get { return gravity; }
    }
    #endregion

    #region Dash Variables
    [Space(10), Header("Dash Settings")]
    [SerializeField] float dashSpeed; //speed of the dash
    [SerializeField] float maxDashSpeed, minDashSpeed; //sets the max and min speed of the dash
    public float DashSpeed //allows other classes to access the dash speed
    {
        get { return dashSpeed; }
        set
        {
            dashSpeed = Mathf.Clamp(value, minDashSpeed, maxDashSpeed);
        }
    }
    [SerializeField] float dashTime; //amount of time spent dashing
    public float DashTime //allows other classes to access the dash time
    {
        get { return dashTime; }
    }
    [SerializeField] float dashCooldown; //amount of time between dashes
    public float DashCooldown //allows other classes to access the dash cooldown
    {
        get { return dashCooldown; }
        set
        {
            dashCooldown = value;
        }
    }
    public GameObject dashEffect;
    public bool canDash = true, dashed;
    #endregion

    #region Ground Check Variables
    [Space(10), Header("Ground Check Settings:")]
    public Transform groundCheckPoint; //point at which ground check happens
    [SerializeField] float groundCheckY = 0.2f; //how far down from ground chekc point is Grounded() checked
    public float GroundCheckY //allows other classes to access the ground check y value
    {
        get { return groundCheckY; }
    }
    [SerializeField] float groundCheckX = 0.5f; //how far horizontally from ground chekc point to the edge of the player is
    public float GroundCheckX //allows other classes to access the ground check x value
    {
        get { return groundCheckX; }
    }
    public LayerMask whatIsGround; //sets the ground layer
    #endregion

    #region Attack Variables
    [Header("Attack Settings:")]
    public Transform SideAttackTransform; //the middle of the side attack area
    public Vector2 SideAttackArea; //how large the area of side attack is

    public Transform UpAttackTransform; //the middle of the up attack area
    public Vector2 UpAttackArea; //how large the area of side attack is

    public Transform DownAttackTransform; //the middle of the down attack area
    public Vector2 DownAttackArea; //how large the area of down attack is

    public LayerMask attackableLayer; //the layer the player can attack and recoil off of

    public float timeBetweenAttack;
    public float timeSinceAttack;

    [SerializeField] float damage; //the damage the player does to an enemy
    public float Damage //allows other classes to access the damage value
    {
        get { return damage; }
    }

    public GameObject slashEffect; //the effect of the slashs

    public bool restoreTime;
    float restoreTimeSpeed;
    public float RestoreTimeSpeed //allows other classes to access the restore time speed
    {
        get { return restoreTimeSpeed; }
        set
        {
            restoreTimeSpeed = value;
        }
    }
    #endregion

    #region Recoil Variables
    [Space(10), Header("Recoil Settings:")]
    [SerializeField] int recoilXSteps = 5; //how many FixedUpdates() the player recoils horizontally for
    public int RecoilXSteps //allows other classes to access the recoil x steps
    {
        get { return recoilXSteps; }
    }
    [SerializeField] int recoilYSteps = 5; //how many FixedUpdates() the player recoils vertically for
    public int RecoilYSteps //allows other classes to access the recoil y steps
    {
        get { return recoilYSteps; }
    }

    [SerializeField] float recoilXSpeed = 100; //the speed of horizontal recoil
    public float RecoilXSpeed //allows other classes to access the recoil x speed
    {
        get { return recoilXSpeed; }
    }
    [SerializeField] float recoilYSpeed = 100; //the speed of vertical recoil
    public float RecoilYSpeed //allows other classes to access the recoil y speed
    {
        get { return recoilYSpeed; }
    }

    int stepsXRecoiled, stepsYRecoiled; //the no. of steps recoiled horizontally and vertically
    public int StepsXRecoiled //allows other classes to access the no. of steps recoiled horizontally
    {
        get { return stepsXRecoiled; }
        set
        {
            stepsXRecoiled = value;
        }
    }
    public int StepsYRecoiled //allows other classes to access the no. of steps recoiled vertically
    {
        get { return stepsYRecoiled; }
        set
        {
            stepsYRecoiled = value;
        }
    }
    #endregion

    #region Health Variables
    [Space(10), Header("Health Settings")]
    public int health;
    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);

                onHealthChangedCallback?.Invoke();
            }
        }
    }
    public int maxHealth;
    public GameObject bloodSpurt;
    [SerializeField] float hitFlashSpeed;
    public delegate void OnHealthChangedDelegate();
    [HideInInspector] public OnHealthChangedDelegate onHealthChangedCallback;
    public bool canFlash = true;
    float healTimer;
    public float HealTimer
    {
        get { return healTimer; }
        set
        {
            healTimer = value;
        }
    }
    [SerializeField] float timeToHeal;
    public float TimeToHeal
    {
        get { return timeToHeal; }
    }
    #endregion

    #region Mana Variables
    [Header("Mana Settings")]
    [SerializeField] UnityEngine.UI.Image manaStorage;

    [SerializeField] float mana;
    public float Mana
    {
        get { return mana; }
        set
        {
            if (mana != value)
            {
                mana = Mathf.Clamp(value, 0, 1);
                manaStorage.fillAmount = Mana;
            }
        }
    }
    [SerializeField] float manaDrainSpeed;
    public float ManaDrainSpeed
    {
        get { return manaDrainSpeed; }
    }
    [SerializeField] float manaGain;
    public float ManaGain
    {
        get { return manaGain; }
    }
    #endregion

    #region Spell Variables
    [Header("Spell Settings")]
    //spell stats
    [SerializeField] float manaSpellCost = 0.3f;
    public float ManaSpellCost
    {
        get { return manaSpellCost; }
    }
    [SerializeField] float timeBetweenCast = 0.5f;
    public float TimeBetweenCast
    {
        get { return timeBetweenCast; }
    }
    [SerializeField] float spellDamage; //upspellexplosion and downspellfireball
    public float SpellDamage { get { return spellDamage; } }
    [SerializeField] float downSpellForce; // desolate dive only
    public float DownSpellForce { get { return downSpellForce; } }
    //spell cast objects
    public GameObject sideSpellFireball;
    public GameObject upSpellExplosion;
    public GameObject downSpellFireball;
    float timeSinceCast;
    public float TimeSinceCast
    {
        get { return timeSinceCast; }
        set { timeSinceCast = value; }
    }
    float castOrHealTimer;
    public float CastOrHealTimer
    {
        get { return castOrHealTimer; }
        set { castOrHealTimer = value; }
    }
    #endregion

    #region Input Variables
    [Space(10), Header("Input Settings")]
    public bool attack;
    public float xAxis, yAxis;
    #endregion

    #region Player Components
    [Space(10), Header("Player Components:")]
    Rigidbody2D _rigidBody;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    #endregion


    protected override void Awake()
    {
        base.Awake();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        gravity = _rigidBody.gravityScale;
        SetDefaultMana();
    }

    void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetButtonDown("Attack");

        if (Input.GetButton("Cast/Heal"))
        {
            castOrHealTimer += Time.deltaTime; //TODO erken salındığında timer'ın sıfırlanması lazım
        }
    }

    public void SetLinearVelocity(Vector2 _velocity)
    {
        _rigidBody.linearVelocity = _velocity;
    }

    public Vector2 GetLinearVelocity()
    {
        return _rigidBody.linearVelocity;
    }

    public void SetGravityScale(float _gravityScale)
    {
        _rigidBody.gravityScale = _gravityScale;
    }

    public void SetDefaultMana()
    {
        Mana = mana;
        manaStorage.fillAmount = Mana;
    }

    public void SetMaxHealth()
    {
        health = maxHealth;
    }

    public void SetAnimTrigger(string triggerName)
    {
        if (_animator == null)
            LogSystem.LogError("Animator component not found.");
        _animator.SetTrigger(triggerName);
    }

    public void SetAnimBool(string boolName, bool value)
    {
        if (_animator == null)
            LogSystem.LogError("Animator component not found.");
        _animator.SetBool(boolName, value);
    }

    public void SpriteRendererFlash()
    {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
    }

    public void SetSpriteRenderer(bool state)
    {
        _spriteRenderer.enabled = state;
    }

}
