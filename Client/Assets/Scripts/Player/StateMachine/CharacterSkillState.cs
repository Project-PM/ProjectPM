using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬은 일반 콤보 공격과는 느낌이 다르다
/// 어떤 스펙이 들어갈 지 알 수 없기 때문에... 스펙 보고 스킬마다 구현하는 쪽으로 우선 가자
/// </summary>
/// 
public abstract class CharacterSkillState : CharacterControllerState
{
    protected abstract ENUM_CHARACTER_TYPE skillCharacterType { get; }

    private bool IsValidState => skillCharacterType == characterType;

    public sealed override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (IsValidState == false)
            return;

        OnSkillStateEnter(animator, animatorStateInfo);
    }

    public sealed override void OnStatePrevUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (IsValidState == false)
            return;

        OnSkillStatePrevUpdate(animator, animatorStateInfo);
    }

    public sealed override void OnStateLateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (IsValidState == false)
            return;

        OnSkillStateLateUpdate(animator, animatorStateInfo);
    }

    public sealed override void OnStateExit(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (IsValidState == false)
            return;

        OnSkillStateExit(animator, animatorStateInfo);
    }


    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {

    }

    protected abstract void OnSkillStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo);
    protected abstract void OnSkillStatePrevUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo);
    protected abstract void OnSkillStateLateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo);
    protected abstract void OnSkillStateExit(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo);
}
