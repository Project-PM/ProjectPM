using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUltimateStartState : CharacterControllerState
{
    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (controller.CheckSuccessAttack())
        {
            animator.Play(ENUM_CHARACTER_STATE.Ultimate);
        }
        else if(controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
        {
            if (damageType == ENUM_DAMAGE_TYPE.Stand)
            {
                animator.Play(ENUM_CHARACTER_STATE.StandHit1);
            }
            else if (damageType == ENUM_DAMAGE_TYPE.Airborne)
            {
                animator.Play(ENUM_CHARACTER_STATE.AirborneHit1);
            }
        }
        else
        {
            if (IsEndState(animatorStateInfo))
            {
                animator.Play(ENUM_CHARACTER_STATE.Idle);
            }
        }
    }
}
