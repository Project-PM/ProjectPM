using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFallState : CharacterControllerState
{
	[SerializeField] private float moveSpeed = 0.1f;

	public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, animatorStateInfo, layerIndex);
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
		else if (controller.CheckSkill())
		{
			animator.Play("JumpSkill");
		}
		else if (controller.CheckGrounded())
		{
			animator.Play("Land");
		}
	}
}
