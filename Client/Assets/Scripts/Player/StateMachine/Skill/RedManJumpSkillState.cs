using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedManJumpSkillState : CharacterSkillState
{
    [SerializeField] private Vector2 moveSpeedVec = Vector2.zero;

    protected override ENUM_CHARACTER_TYPE skillCharacterType => ENUM_CHARACTER_TYPE.Red;

    protected override void OnSkillStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        controller.TrySetGravity(false);
        TryMove();
    }

    protected override void OnSkillStateExit(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        controller.TrySetGravity(true);
    }

    protected override void OnSkillStateLateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        
    }

    protected override void OnSkillStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        TryMove();
    }

    private void TryMove()
    {
        Vector2 moveVec = Vector2.zero;

        if (controller.CheckFrontRight())
        {
            moveVec = moveSpeedVec;
        }
        else
        {
            moveVec = new Vector2(moveSpeedVec.x * -1, moveSpeedVec.y);
        }

        controller.MovePosition(moveVec);

    }

    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (controller.CheckHit(out var damageType))
        {
            animator.Play(ENUM_CHARACTER_STATE.AirborneHit1);
        }
        else if (controller.CheckGrounded())
        {
            animator.Play(ENUM_CHARACTER_STATE.JumpSkillLand);
        }
    }
}
