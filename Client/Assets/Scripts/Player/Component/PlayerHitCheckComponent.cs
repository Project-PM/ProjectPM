using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCheckComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerCharacterController controller;

    public bool isInvinsible = false;
    public bool isSuperArmor = false;

    private ENUM_DAMAGE_TYPE currentDamagedType = ENUM_DAMAGE_TYPE.None;

    private void OnEnable()
    {
        controller.IsHit += IsHit;
    }

    private void OnDisable()
    {
        controller.IsHit -= IsHit;
    }

    private ENUM_DAMAGE_TYPE IsHit()
    {
        return currentDamagedType;
    }

    private void LateUpdate()
    {
        currentDamagedType = ENUM_DAMAGE_TYPE.None;
    }

    public bool OnHit(PlayerCharacterController attacker, DamageInfo damageInfo)
    {
        if (isInvinsible)
        {
            currentDamagedType = ENUM_DAMAGE_TYPE.None;
            return false;
        }

        currentDamagedType = isSuperArmor ? ENUM_DAMAGE_TYPE.JustDamage : damageInfo.type;
        return true;
    }
}
