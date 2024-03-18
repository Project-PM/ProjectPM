using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumpState : CharacterControllerState
{
	[SerializeField] private float moveSpeed = 0.1f;
	[SerializeField] private float jumpHeight = 0.1f;
	[SerializeField] private float attackableHeight = 0.1f;

	public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, animatorStateInfo, layerIndex);
		controller.TryJump(jumpHeight);
		controller.TryMoveAndJump(moveSpeed);
	}

	public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStatePrevUpdate(animator, animatorStateInfo, layerIndex);
		controller.TryMoveAndJump(moveSpeed);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		controller.TryMoveAndJump(moveSpeed);
		base.OnStateExit(animator, animatorStateInfo, layerIndex);
	}
	
	private bool IsAttackableHeight()
	{
		return attackableHeight <= controller.GetHeightFromGround();
	}

	protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		// 뭘 맞아도 이 때는 공중 피격
		if (controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
		{
			animator.Play(ENUM_CHARACTER_STATE.AirborneHit1);
		}
		else if (controller.CheckAttack() && IsAttackableHeight())
		{
			animator.Play(ENUM_CHARACTER_STATE.JumpAttack);
		}
		else if(controller.CheckSkill() && IsAttackableHeight())
		{
			animator.Play(ENUM_CHARACTER_STATE.JumpSkill);
		}
		else if (controller.CheckFall())
		{
			if (animatorStateInfo.IsEndState())
			{
				animator.Play(ENUM_CHARACTER_STATE.Fall);
			}
		}
		else if (controller.CheckGrounded())
		{
			animator.Play(ENUM_CHARACTER_STATE.Land);
		}
	}

}
