using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGuardState : CharacterControllerState
{
    [SerializeField] private float guardKnockBackDistance;

    public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        TryKnockBack(); 
    }

    public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStatePrevUpdate(animator, animatorStateInfo, layerIndex);
        TryKnockBack();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        TryKnockBack();
        base.OnStateExit(animator, animatorStateInfo, layerIndex);
    }

    private void TryKnockBack()
    {
        if (controller.CheckHit(out ENUM_DAMAGE_TYPE _) == false)
            return;

        Vector2 knockBackMoveVec = Vector2.zero;

        if (controller.CheckFrontRight())
        {
            knockBackMoveVec = new Vector2(guardKnockBackDistance * -1, 0);
        }
        else
        {
            knockBackMoveVec = new Vector2(guardKnockBackDistance, 0);
        }

        controller.MovePosition(knockBackMoveVec);
    }

    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
        {
            if (damageType != ENUM_DAMAGE_TYPE.GuardDamage)
            {
                animator.Play(ENUM_CHARACTER_STATE.StandHit1);
            }
        }
        else if(controller.CheckGuard() == false)
        {
            animator.Play(ENUM_CHARACTER_STATE.Idle);
        }
    }
}
