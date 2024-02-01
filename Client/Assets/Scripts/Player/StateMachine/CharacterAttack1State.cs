using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack1State : CharacterControllerState
{
	public override void OnStateLateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateLateUpdate(animator, animatorStateInfo, layerIndex);

		if (IsEndState(animatorStateInfo))
		{
			animator.Play("Idle");
		}
	}

}
