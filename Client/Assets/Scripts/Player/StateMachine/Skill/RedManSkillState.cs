using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedManSkillState : CharacterSkillState
{
    protected override ENUM_CHARACTER_TYPE skillCharacterType => ENUM_CHARACTER_TYPE.Red;

    protected override void OnSkillStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        
    }

    protected override void OnSkillStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        
    }

    protected override void OnSkillStateLateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        
    }

    protected override void OnSkillStateExit(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        
    }

    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
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
        else if (animatorStateInfo.IsEndState())
        {
            animator.Play(ENUM_CHARACTER_STATE.Idle);
        }
    }
}
