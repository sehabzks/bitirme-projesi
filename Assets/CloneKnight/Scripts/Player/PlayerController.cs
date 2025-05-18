using System.Collections;
using UnityEngine;

public class PlayerController : PersistentSingleton<PlayerController>
{
    [HideInInspector] public PlayerStateList pState;
    PlayerData playerData;

    void Start()
    {
        playerData = PlayerData.Instance;
        pState = GetComponent<PlayerStateList>();
        playerData.SetDefaultMana();
        playerData.SetMaxHealth();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(playerData.SideAttackTransform.position, playerData.SideAttackArea);
        //Gizmos.DrawWireCube(playerData.UpAttackTransform.position, playerData.UpAttackArea);
        //Gizmos.DrawWireCube(playerData.DownAttackTransform.position, playerData.DownAttackArea);
    }

    void Update()
    {
        if (pState.cutscene) return;
        if (pState.dashing) return;
        RestoreTimeScale();
        FlashWhileInvincible();
    }

    private void OnTriggerEnter2D(Collider2D _other) //for up and down cast spell
    {
        var enemy = _other.GetComponent<Enemy>();
        if (!pState.casting) return;
        if (enemy == null) return;
        enemy.EnemyHit(playerData.SpellDamage, (_other.transform.position - transform.position).normalized, -playerData.RecoilYSpeed);
    }



    void RestoreTimeScale()
    {
        if (!playerData.restoreTime) return;
        if (Time.timeScale < 1)
        {
            Time.timeScale += Time.unscaledDeltaTime * playerData.RestoreTimeSpeed;
            return;
        }
        Time.timeScale = 1;
        playerData.restoreTime = false;
    }
    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        playerData.RestoreTimeSpeed = _restoreSpeed;
        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));

        }
        else
        {
            playerData.restoreTime = true;
        }
        Time.timeScale = _newTimeScale;
    }
    IEnumerator StartTimeAgain(float _delay)
    {
        yield return new WaitForSecondsRealtime(_delay);
        Time.timeScale = 1f;
        playerData.restoreTime = false;
    }


    IEnumerator Flash()
    {
        playerData.SpriteRendererFlash();
        playerData.canFlash = false;
        yield return new WaitForSeconds(0.1f);
        playerData.canFlash = true;
    }
    void FlashWhileInvincible()
    {
        if (pState.invincible && !pState.cutscene && playerData.canFlash && Time.timeScale > 0.2)
        {
            StartCoroutine(Flash());
            return;
        }
        playerData.SetSpriteRenderer(true);
    }


}