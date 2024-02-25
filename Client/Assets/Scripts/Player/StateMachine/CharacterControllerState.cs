using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ENUM_CHARACTER_STATE
{
	Idle,
	FrontMove,
	BackMove,
	Fall,
	Jump,
	Land,
	AirborneHit1,
	StandHit1,
	Down,
	Recovery,
	Attack1,
	Attack2,
	Attack3,
	JumpAttack,
	Skill,
	JumpSkill,
	Ultimate_Start,
	Ultimate,
	Guard,
}

public static class AnimatorHelper
{
	public static void Play(this Animator animator, ENUM_CHARACTER_STATE characterState)
	{
		animator.Play(characterState.ToString());
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
