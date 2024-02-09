using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCheckComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerCharacterController controller;

    [SerializeField] private float superArmorDamageRate = 1.2f;
    [SerializeField] private float guardDamageRate = 0.3f;

    public bool isInvinsible = false;
    public bool isSuperArmor = false;
    public bool isGuard = false;

    private Coroutine endOfFrameCoroutine = null;
    private WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    private ENUM_DAMAGE_TYPE currentDamagedType = ENUM_DAMAGE_TYPE.None;

    private void OnEnable()
    {
        controller.IsHit += IsHit;

        if (endOfFrameCoroutine != null)
        {
            StopCoroutine(endOfFrameCoroutine);
        }

        endOfFrameCoroutine = StartCoroutine(OnLateUpdate());
    }

    private void OnDisable()
    {
        if (endOfFrameCoroutine != null)
        {
            StopCoroutine(endOfFrameCoroutine);
        }

        controller.IsHit -= IsHit;
    }

    private ENUM_DAMAGE_TYPE IsHit()
    {
        return currentDamagedType;
    }

    private IEnumerator OnLateUpdate()
    {
        while(true)
        {
            yield return endOfFrame;
            currentDamagedType = ENUM_DAMAGE_TYPE.None;
        }
    }

    public bool OnHit(PlayerCharacterController attacker, DamageInfo damageInfo)
    {
        if (isInvinsible)
        {
            currentDamagedType = ENUM_DAMAGE_TYPE.None;
            return false;
        }

        if (isGuard)
        {
            currentDamagedType = ENUM_DAMAGE_TYPE.GuardDamage;
        }
        else if (isSuperArmor)
        {
            currentDamagedType = ENUM_DAMAGE_TYPE.JustDamage;
        }
        else
        {
            currentDamagedType = damageInfo.type;
        }

        return true;
    }
}
