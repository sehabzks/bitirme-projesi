using System.Collections;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    PlayerData playerData;
    PlayerMovement playerMovement;
    PlayerStateList pState;

    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        playerData = PlayerData.Instance;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CastFireball();
    }

    void FixedUpdate()
    {
        if (pState.dashing || pState.healing || pState.cutscene) return;
        Recoil();
    }


    void CastFireball()
    {
        if (Input.GetButtonUp("Cast/Heal") && playerData.CastOrHealTimer <= 0.05f && playerData.TimeSinceCast >= playerData.TimeBetweenCast && playerData.Mana >= playerData.ManaSpellCost)
        {
            pState.casting = true;
            playerData.TimeSinceCast = 0;
            StartCoroutine(CastFireballCoroutine());
        }
        else
        {
            playerData.TimeSinceCast += Time.deltaTime;
        }

        if (!Input.GetButton("Cast/Heal"))
        {
            playerData.CastOrHealTimer = 0;
        }

        if (playerMovement.IsGrounded())
        {
            //disable downspell if on the ground
            playerData.downSpellFireball.SetActive(false);
        }
        //if down spell is active, force player down until grounded
        if (playerData.downSpellFireball.activeInHierarchy)
        {
            playerData.SetLinearVelocity(playerData.GetLinearVelocity() + playerData.DownSpellForce * Vector2.down);
        }
    }

    IEnumerator CastFireballCoroutine()
    {
        playerData.SetAnimBool("Casting", true);
        yield return new WaitForSeconds(0.15f);

        //side cast
        if (playerData.yAxis == 0 || (playerData.yAxis < 0 && playerMovement.IsGrounded()))
        {
            GameObject _fireBall = Instantiate(playerData.sideSpellFireball, playerData.SideAttackTransform.position, Quaternion.identity);

            //flip fireball
            if (pState.lookingRight)
            {
                _fireBall.transform.eulerAngles = Vector3.zero; // if facing right, fireball continues as per normal
            }
            else
            {
                _fireBall.transform.eulerAngles = new Vector2(_fireBall.transform.eulerAngles.x, 180);
                //if not facing right, rotate the fireball 180 deg
            }
            pState.recoilingX = true;
        }

        //up cast
        else if (playerData.yAxis > 0)
        {
            Instantiate(playerData.upSpellExplosion, transform);
            playerData.SetLinearVelocity(Vector2.zero);
        }

        //down cast
        else if (playerData.yAxis < 0 && !playerMovement.IsGrounded())
        {
            playerData.downSpellFireball.SetActive(true);
        }

        playerData.Mana -= playerData.ManaSpellCost;
        yield return new WaitForSeconds(0.35f);
        playerData.SetAnimBool("Casting", false);
        pState.casting = false;
    }


    public void Recoil()
    {
        if (pState.recoilingX)
        {
            var _recoilDirX = pState.lookingRight ? -1 : 1;
            playerData.SetLinearVelocity(new Vector2(_recoilDirX * playerData.RecoilXSpeed, 0)); //! 0 yerine playerData.getvelocity().y yazılabilir mi?
        }

        if (pState.recoilingY)
        {
            playerData.SetGravityScale(0);
            var _recoilDirY = playerData.yAxis < 0 ? 1 : -1;
            playerData.SetLinearVelocity(new Vector2(playerData.GetLinearVelocity().x, _recoilDirY * playerData.RecoilYSpeed));
            playerData.AirJumpCounter = 0;
        }
        else
        {
            playerData.SetGravityScale(playerData.Gravity);
        }

        //stop recoil
        if (pState.recoilingX && playerData.StepsXRecoiled < playerData.RecoilXSteps)
        {
            playerData.StepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY && playerData.StepsYRecoiled < playerData.RecoilYSteps)
        {
            playerData.StepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (playerMovement.IsGrounded())
        {
            StopRecoilY();
        }
    }

    void StopRecoilX()
    {
        playerData.StepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY()
    {
        playerData.StepsYRecoiled = 0;
        pState.recoilingY = false;
    }
}
