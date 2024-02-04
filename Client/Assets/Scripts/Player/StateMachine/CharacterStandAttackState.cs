using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStandAttackState : CharacterControllerState
{
	[SerializeField] private float comboNormalizeTime = 0.9f;
	[SerializeField] private ENUM_CHARACTER_STATE nextComboState = ENUM_CHARACTER_STATE.Attack2;

	protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		if (controller.CheckAttack() && animatorStateInfo.IsState(ENUM_CHARACTER_STATE.Attack3) == false && animatorStateInfo.normalizedTime >= comboNormalizeTime)
		{
			animator.Play(nextComboState);
		}
		else if (animatorStateInfo.IsEndState())
		{
			animator.Play(ENUM_CHARACTER_STATE.Idle);
		}
	}
}
