using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENUM_DAMAGE_TYPE
{
    Stand,
    Airborne,
    Down
}

[Serializable]
public struct DamageInfo
{
    [Header("[데미지 타입]")]
    [SerializeField] public ENUM_DAMAGE_TYPE type;

    [Header("[기본 데미지 양]")]
    [SerializeField] public int amount;

    [Header("[밀어내는 힘]")]
    [SerializeField] public float knockBackPower;

    [Header("[경직 시간 (데미지 타입이 Stand인 경우에만 유효)]")]
    [SerializeField] public float stiffTime;

    [Header("[띄우는 힘 (데미지 타입이 Airborne인 경우에만 유효)]")]
    [SerializeField] public float upperPower;
}

public interface IDamageable
{
	bool OnHit(PlayerCharacterController attacker, DamageInfo damageInfo);
}