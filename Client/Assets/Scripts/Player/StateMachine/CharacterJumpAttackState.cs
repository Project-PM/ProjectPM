using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumpAttackState : CharacterFallState
{
    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
        {
            animator.Play(ENUM_CHARACTER_STATE.AirborneHit1);
        }
        else if (controller.CheckGrounded())
        {
            animator.Play(ENUM_CHARACTER_STATE.Land);
        }
    }
}
