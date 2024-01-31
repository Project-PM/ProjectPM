using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriggerInfo
{
	[SerializeField] private string stateName;
	[SerializeField] private int startFrameIndex;

	public bool Equals(string stateName, int startFrameCount)
	{
		return this.stateName == stateName && this.startFrameIndex == startFrameCount;
	}
}

public abstract class PlayerFrameActionComponent : MonoBehaviour
{
    [SerializeField] private int deltaFrameCount = 1;
	[SerializeField] private List<TriggerInfo> triggers = new List<TriggerInfo>();

	private AnimationFrameReceiver receiver;
	protected int DeltaFrameCount => deltaFrameCount;
	private ENUM_CHARACTER_TYPE playerCharacterType = ENUM_CHARACTER_TYPE.None;
	protected abstract ENUM_CHARACTER_TYPE componentCharacterType { get; }

	private void Awake()
	{
		receiver = GetComponentInParent<AnimationFrameReceiver>();
	}

	private void OnEnable()
	{
		receiver.onChangedFrame += OnChangedFrame;
	}

	private void OnDisable()
	{
		receiver.onChangedFrame -= OnChangedFrame;
	}

	public void SetPlayerCharacterType(ENUM_CHARACTER_TYPE characterType)
	{
		this.playerCharacterType = characterType;
	}

	private void OnChangedFrame(string stateName, int frameIndex)
	{
		if (playerCharacterType != componentCharacterType)
			return;

		foreach (var trigger in triggers)
		{
			if (trigger.Equals(stateName, frameIndex))
			{
				DoAction();
				break;
			}
		}
	}

	protected abstract void DoAction();
}


public class RedManAttackComponent : PlayerFrameActionComponent
{
	protected override ENUM_CHARACTER_TYPE componentCharacterType => ENUM_CHARACTER_TYPE.Red;
	private int remainFrameCount = 0;

	protected override void DoAction()
	{
		remainFrameCount = DeltaFrameCount;
	}

	private void Update()
	{
		if (remainFrameCount == 0)
			return;

		remainFrameCount--;
	}
}
