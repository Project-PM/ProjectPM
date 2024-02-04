using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack1State : CharacterControllerState
{
	protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		if (animatorStateInfo.IsEndState())
		{
			animator.Play(ENUM_CHARACTER_STATE.Idle);
		}
	}
}
