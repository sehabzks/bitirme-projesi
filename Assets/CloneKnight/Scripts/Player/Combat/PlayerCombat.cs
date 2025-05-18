using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : PersistentSingleton<PlayerCombat>
{
    PlayerData playerData;
    PlayerMovement playerMovement;
    PlayerStateList pState;

    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        playerData = PlayerData.Instance;
    }

    void Update()
    {
        if (pState.cutscene) return;
        if (pState.dashing) return;
        Heal();
        if (pState.healing) return;
        Attack();
    }


    void Attack() //! Sanırım biraz temizlenebilir
    {
        playerData.timeSinceAttack += Time.deltaTime;
        if (!playerData.attack || playerData.timeSinceAttack < playerData.timeBetweenAttack) return;

        playerData.timeSinceAttack = 0;
        playerData.SetAnimTrigger("Attacking");

        if (playerData.yAxis == 0 || playerData.yAxis < 0 && playerMovement.IsGrounded())
        {
            Hit(playerData.SideAttackTransform, playerData.SideAttackArea, ref pState.recoilingX, playerData.RecoilXSpeed);
            Instantiate(playerData.slashEffect, playerData.SideAttackTransform);
        }
        else if (playerData.yAxis > 0)
        {
            Hit(playerData.UpAttackTransform, playerData.UpAttackArea, ref pState.recoilingY, playerData.RecoilYSpeed);
            SlashEffectAtAngle(playerData.slashEffect, 80, playerData.UpAttackTransform);
        }
        else if (playerData.yAxis < 0 && !playerMovement.IsGrounded())
        {
            Hit(playerData.DownAttackTransform, playerData.DownAttackArea, ref pState.recoilingY, playerData.RecoilYSpeed);
            SlashEffectAtAngle(playerData.slashEffect, -90, playerData.DownAttackTransform);
        }
    }

    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, playerData.attackableLayer);
        List<Enemy> hitEnemies = new();

        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() == null) return;

            Enemy e = objectsToHit[i].GetComponent<Enemy>();
            if (e && !hitEnemies.Contains(e))
            {
                e.EnemyHit(playerData.Damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
                hitEnemies.Add(e);
            }

            if (objectsToHit[i].CompareTag("Enemy"))
            {
                playerData.Mana += playerData.ManaGain;
            }
        }
    }

    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    public void TakeDamage(float _damage)
    {
        if (pState.invincible) return; // Eğer zaten geçici hasarsızlık varsa, hasar alma

        playerData.Health -= Mathf.RoundToInt(_damage);
        StartCoroutine(StopTakingDamage());
    }


    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        GameObject _bloodSpurtParticles = Instantiate(playerData.bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurtParticles, 1.5f);
        playerData.SetAnimTrigger("TakeDamage");
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
    }

    void Heal()
    {
        if (Input.GetButton("Cast/Heal") && playerData.Health < playerData.maxHealth && playerData.Mana > 0 && playerMovement.IsGrounded() && !pState.dashing)
        {
            pState.healing = true;
            playerData.SetAnimBool("Healing", true);

            //healing
            playerData.HealTimer += Time.deltaTime;
            if (playerData.HealTimer >= playerData.TimeToHeal)
            {
                playerData.Health++;
                playerData.HealTimer = 0;
            }

            //drain mana
            playerData.Mana -= Time.deltaTime * playerData.ManaDrainSpeed;
            return;
        }

        pState.healing = false;
        playerData.SetAnimBool("Healing", false);
        playerData.HealTimer = 0;
    }
}
