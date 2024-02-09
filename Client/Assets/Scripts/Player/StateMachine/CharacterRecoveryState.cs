using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRecoveryState : CharacterControllerState
{
    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        if (IsEndState(animatorStateInfo))
        {
            animator.Play(ENUM_CHARACTER_STATE.Idle);
        }
    }
}
