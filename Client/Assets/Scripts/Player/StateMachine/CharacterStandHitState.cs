using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStandHitState : CharacterControllerState
{
	[SerializeField] private ENUM_CHARACTER_STATE characterState = ENUM_CHARACTER_STATE.StandHit1;

	public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        OnStandHit();
    }

    private void OnStandHit()
    {
        var damageInfo = GetDamageInfo();

        float moveX = controller.CheckFrontRight() ? damageInfo.knockBackPower * -1 : damageInfo.knockBackPower;
        controller.MovePosition(moveX, 0.0f);
    }

    private DamageInfo GetDamageInfo()
    {
        return controller.TryGetDamageInfo();
    }

    protected override bool IsEndState(AnimatorStateInfo stateInfo)
    {
        var damageInfo = GetDamageInfo();
        float hitDeltaTime = stateInfo.length * stateInfo.normalizedTime;
        return hitDeltaTime >= damageInfo.stiffTime;
    }

    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
        {
            if (damageType == ENUM_DAMAGE_TYPE.Stand)
            {
                if(characterState == ENUM_CHARACTER_STATE.StandHit1)
                {
					animator.Play(ENUM_CHARACTER_STATE.StandHit2);
				}
				else if(characterState == ENUM_CHARACTER_STATE.StandHit2)
                {
					animator.Play(ENUM_CHARACTER_STATE.StandHit1);
				}
			}
            else if (damageType == ENUM_DAMAGE_TYPE.Airborne)
            {
                animator.Play(ENUM_CHARACTER_STATE.AirborneHit1);
            }
        }
        else if(IsEndState(animatorStateInfo))
        {
            animator.Play(ENUM_CHARACTER_STATE.Idle);
        }
    }
}
