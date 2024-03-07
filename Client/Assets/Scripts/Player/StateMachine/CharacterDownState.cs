using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDownState : CharacterControllerState
{
    [SerializeField] private float upperPowerRate = 0.3f;

    public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        OnDownHit();
    }

    public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStatePrevUpdate(animator, animatorStateInfo, layerIndex);
        controller.TryMoveAndJump();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        controller.TryMoveAndJump();
        base.OnStateExit(animator, animatorStateInfo, layerIndex);
    }

    private void OnDownHit()
    {
        var damageInfo = GetDamageInfo();
        controller.TryJump(damageInfo.upperHeight * upperPowerRate);

        float knockBackPower = controller.CheckFrontRight() ? damageInfo.knockBackPower * -1 : damageInfo.knockBackPower;
        controller.TryMoveAndJump(knockBackPower * upperPowerRate);
    }

    private DamageInfo GetDamageInfo()
    {
        return controller.TryGetDamageInfo();
    }

    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
        {
            if (damageType != ENUM_DAMAGE_TYPE.JustDamage)
            {
                animator.Play(ENUM_CHARACTER_STATE.Down);
            }
        }
        else if (IsEndState(animatorStateInfo) && controller.CheckGrounded())
        {
            animator.Play(ENUM_CHARACTER_STATE.Recovery);
        }
    }
}
