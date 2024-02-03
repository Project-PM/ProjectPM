using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이트 추가하고 애니메이터 플레이 대체하기
public enum ENUM_CHARACTER_STATE
{

}

public class CharacterControllerState : StateMachineBehaviour
{
	protected PlayerCharacterController controller = null;
	protected ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.None;

	public static void Initialize(ENUM_CHARACTER_TYPE characterType, Animator ownerAnimator)
	{
		var controller = ownerAnimator.GetComponent<PlayerCharacterController>();
		if (controller == null)
			return;

		var states = ownerAnimator.GetBehaviours<CharacterControllerState>();
		foreach (var state in states)
		{
			state.InitializeInternal(characterType, controller);
		}
	}

	private void InitializeInternal(ENUM_CHARACTER_TYPE characterType, PlayerCharacterController controller)
	{
		this.characterType = characterType;
		this.controller = controller;
	}

	protected bool IsEndState(AnimatorStateInfo stateInfo)
	{
		return stateInfo.normalizedTime >= 0.99f;
	}

	public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, animatorStateInfo, layerIndex);
	}

	public sealed override void OnStateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		OnStatePrevUpdate(animator, animatorStateInfo, layerIndex);

		base.OnStateUpdate(animator, animatorStateInfo, layerIndex);
		CheckNextState(animator, animatorStateInfo);

		OnStateLateUpdate(animator, animatorStateInfo, layerIndex);
	}

	public virtual void OnStatePrevUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{

	}

	public virtual void OnStateLateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{

	}

	public override void OnStateExit(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateExit(animator, animatorStateInfo, layerIndex);
	}

	public sealed override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		base.OnStateMachineEnter(animator, stateMachinePathHash);
	}

	public sealed override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		base.OnStateMachineExit(animator, stateMachinePathHash);
	}

	protected virtual void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		if (controller.CheckHit(out DamageType damageType))
		{
			if (damageType == DamageType.Stand)
			{
				animator.Play("StandHit");
			}
			else if (damageType == DamageType.Airborne)
			{
				animator.Play("AirborneHit");
			}
			else if (damageType == DamageType.Down)
			{
				animator.Play("Down");
			}
		}
		else if (controller.CheckUltimate())
		{
			animator.Play("Ultimate");
		}
		else if (controller.CheckSkill())
		{
			animator.Play("Skill");
		}
		else if (controller.CheckAttack())
		{
			animator.Play("Attack1");
		}
		else if (controller.CheckJumpable())
		{
			animator.Play("Jump");
		}
		else if (controller.CheckMove(out bool isFront))
		{
			if (isFront)
			{
				animator.Play("FrontMove");
			}
			else
			{
				animator.Play("BackMove");
			}
		}
		else if (controller.CheckFall())
		{
			animator.Play("Fall");
		}
		else if (animatorStateInfo.IsName("Idle") == false)
		{
			animator.Play("Idle");
		}
	}
}
