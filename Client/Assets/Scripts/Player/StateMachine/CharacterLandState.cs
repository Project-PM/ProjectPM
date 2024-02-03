using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLandState : CharacterControllerState
{
	protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		if (animatorStateInfo.normalizedTime >= 1.0f)
		{
			animator.Play("Idle");
		}
	}

}
