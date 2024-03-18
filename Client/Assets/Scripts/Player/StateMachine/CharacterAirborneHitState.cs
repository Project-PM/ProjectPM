using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAirborneHitState : CharacterControllerState
{
    [SerializeField] private ENUM_CHARACTER_STATE characterState = ENUM_CHARACTER_STATE.AirborneHit1;

	public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        OnAirborneHit();
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

    private void OnAirborneHit()
    {
        var damageInfo = GetDamageInfo();

        controller.TryJump(damageInfo.upperHeight);

        float moveX = controller.CheckFrontRight() ? damageInfo.knockBackPower * -1 : damageInfo.knockBackPower;
        controller.MovePosition(moveX, 0);
    }

    private DamageInfo GetDamageInfo()
    {
        return controller.TryGetDamageInfo();
    }

    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
        {
            if (damageType == ENUM_DAMAGE_TYPE.Airborne)
            {
                if(characterState == ENUM_CHARACTER_STATE.AirborneHit1)
                {
					animator.Play(ENUM_CHARACTER_STATE.AirborneHit2);
				}
                else if(characterState == ENUM_CHARACTER_STATE.AirborneHit2)
                {
					animator.Play(ENUM_CHARACTER_STATE.AirborneHit1);
				}
			}
        }
        else if (controller.CheckGrounded() && stateDeltaTime > 0.1f)
        {
            animator.Play(ENUM_CHARACTER_STATE.Down);
        }
    }
}
