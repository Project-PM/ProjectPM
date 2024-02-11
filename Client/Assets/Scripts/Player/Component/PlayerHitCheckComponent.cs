using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCheckComponent : PlayerChildComponent, IDamageable
{
    [SerializeField] private float superArmorDamageRate = 1.2f;
    [SerializeField] private float guardDamageRate = 0.3f;

    public bool isInvinsible = false;
    public bool isSuperArmor = false;
    public bool isGuard = false;

    private Coroutine endOfFrameCoroutine = null;
    private WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    private ENUM_DAMAGE_TYPE currentDamagedType = ENUM_DAMAGE_TYPE.None;
    private DamageInfo currentDamageInfo = new DamageInfo();

    private void OnEnable()
    {
        controller.IsHit += IsHit;
        controller.GetDamageInfo += GetDamageInfo;

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
        controller.GetDamageInfo -= GetDamageInfo;
    }

    private ENUM_DAMAGE_TYPE IsHit()
    {
        return currentDamagedType;
    }

    private DamageInfo GetDamageInfo()
    {
        return currentDamageInfo;
    }

    private IEnumerator OnLateUpdate()
    {
        while(true)
        {
            yield return endOfFrame;
            currentDamagedType = ENUM_DAMAGE_TYPE.None;
        }
    }

    // 애니메이션 업데이트에서 불림
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

        currentDamageInfo = damageInfo;
        return true;
    }
}
