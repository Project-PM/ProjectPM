using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumpState : CharacterControllerState
{
	[SerializeField] private float moveSpeed = 0.1f;
	[SerializeField] private float jumpHeight = 0.1f;

	public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, animatorStateInfo, layerIndex);
		controller.TryJump(jumpHeight);
		controller.TryMove(moveSpeed);
	}

	public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStatePrevUpdate(animator, animatorStateInfo, layerIndex);
		controller.TryMove(moveSpeed);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		controller.TryMove(moveSpeed);
		base.OnStateExit(animator, animatorStateInfo, layerIndex);
	}

	protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		// 뭘 맞아도 이 때는 공중 피격
		if (controller.CheckHit(out DamageType damageType))
		{
			animator.Play("AirborneHit");
		}
		else if (controller.CheckAttack())
		{
			animator.Play("JumpAttack");
		}
		else if(controller.CheckSkill())
		{
			animator.Play("JumpSkill");
		}
		else if (controller.CheckFall())
		{
			if (animatorStateInfo.normalizedTime >= 1.0f)
			{
				animator.Play("Fall");
			}
		}
		else if (controller.CheckGrounded())
		{
			animator.Play("Land");
		}
	}

}
