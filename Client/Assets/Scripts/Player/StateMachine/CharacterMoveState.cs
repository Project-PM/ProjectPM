using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveState : CharacterControllerState
{
    [SerializeField] private float moveSpeed = 0.1f;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, animatorStateInfo, layerIndex);
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
}
