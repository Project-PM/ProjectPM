using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ENUM 사이에 추가하지 말 것
/// </summary>
public enum ENUM_CHARACTER_STATE
{
	Idle = 0,
	FrontMove = 1,
	BackMove = 2,
	Fall = 3,
	Jump = 4,
	Land = 5,
	AirborneHit1 = 6,
	StandHit1 = 7,
	Down = 8,
	Recovery = 9,
	Attack1 = 10,
	Attack2 = 11,
	Attack3 = 12,
	JumpAttack = 13,
	Skill = 14,
	JumpSkill = 15,
	JumpSkillLand = 16,
	Ultimate_Start = 17,
	Ultimate = 18,
	Guard = 19,
	AirborneHit2 = 20,
	StandHit2 = 21,
}

public static class AnimatorHelper
{
	public static void Play(this Animator animator, ENUM_CHARACTER_STATE characterState)
	{
		animator.Play(characterState.ToString());
	}

	public static void Play(this Animator animator, ENUM_CHARACTER_STATE characterState, float normalizedTime)
	{
		animator.Play(characterState.ToString(), 0, normalizedTime);
	}

	public static bool IsState(this Animator animator, ENUM_CHARACTER_STATE characterState)
	{
		return IsState(animator.GetCurrentAnimatorStateInfo(0), characterState);
	}

	public static bool IsState(this AnimatorStateInfo stateInfo,  ENUM_CHARACTER_STATE characterState)
	{
		return stateInfo.IsName(characterState.ToString());
	}

	public static bool IsEndState(this AnimatorStateInfo stateInfo)
	{
		return stateInfo.normalizedTime >= 1.0f;
	}

	public static void InitializeCharacter(this Animator ownerAnimator, ENUM_CHARACTER_TYPE characterType)
	{
        var controller = ownerAnimator.GetComponent<PlayerCharacterController>();
        if (controller == null)
            return;

        var states = ownerAnimator.GetBehaviours<CharacterControllerState>();
        foreach (var state in states)
        {
            state.InitializeCharacter(characterType, controller);
        }
    }
}

public class CharacterControllerState : StateMachineBehaviour
{
	protected PlayerCharacterController controller = null;
	protected ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.None;

	protected float stateDeltaTime = 0.0f;

	public void InitializeCharacter(ENUM_CHARACTER_TYPE characterType, PlayerCharacterController controller)
	{
		this.characterType = characterType;
		this.controller = controller;
	}

	protected virtual bool IsEndState(AnimatorStateInfo stateInfo)
	{
		return stateInfo.normalizedTime >= 0.99f;
	}

	public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, animatorStateInfo, layerIndex);

		stateDeltaTime = Time.deltaTime;
	}

	public sealed override void OnStateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		OnStatePrevUpdate(animator, animatorStateInfo, layerIndex);

		base.OnStateUpdate(animator, animatorStateInfo, layerIndex);
		CheckNextState(animator, animatorStateInfo);

		OnStateLateUpdate(animator, animatorStateInfo, layerIndex);

		stateDeltaTime += Time.deltaTime;
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

		stateDeltaTime = 0.0f;
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
		if (controller.CheckHit(out ENUM_DAMAGE_TYPE damageType))
		{
			if (damageType == ENUM_DAMAGE_TYPE.Stand)
			{
				animator.Play(ENUM_CHARACTER_STATE.StandHit1);
			}
			else if (damageType == ENUM_DAMAGE_TYPE.Airborne)
			{
				animator.Play(ENUM_CHARACTER_STATE.AirborneHit1);
			}
		}
		else if (controller.CheckUltimate())
		{
			animator.Play(ENUM_CHARACTER_STATE.Ultimate_Start);
		}
		else if (controller.CheckSkill())
		{
			animator.Play(ENUM_CHARACTER_STATE.Skill);
		}
		else if(controller.CheckGuard())
		{
			animator.Play(ENUM_CHARACTER_STATE.Guard);
		}
		else if (controller.CheckAttack())
		{
			animator.Play(ENUM_CHARACTER_STATE.Attack1);
		}
		else if (controller.CheckJumpable())
		{
			animator.Play(ENUM_CHARACTER_STATE.Jump);
		}
		else if (controller.CheckMove(out bool isFront))
		{
			if (isFront)
			{
				animator.Play(ENUM_CHARACTER_STATE.FrontMove);
			}
			else
			{
				animator.Play(ENUM_CHARACTER_STATE.BackMove);
			}
		}
		else if (controller.CheckFall())
		{
			animator.Play(ENUM_CHARACTER_STATE.Fall);
		}
		else if (animatorStateInfo.IsState(ENUM_CHARACTER_STATE.Idle) == false)
		{
			animator.Play(ENUM_CHARACTER_STATE.Idle);
		}
	}
}
