using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationFrameReceiver : MonoBehaviour
{
    [SerializeField] private Animator animator;

	[Header("�ִϸ����Ϳ��� �����Ǵ� ����")]
	public int frameIndex = -1;

	private int prevFrameIndex = -999;
	private string prevStateName = string.Empty;

	public event Action<string, int> onChangedFrame = null;

	private void Update()
	{
		string currentClipName = GetCurrentClipName();
		if (currentClipName == prevStateName && prevFrameIndex == frameIndex)
			return;

		onChangedFrame?.Invoke(currentClipName, frameIndex);

		prevStateName = currentClipName;
		prevFrameIndex = frameIndex;
	}

	private string GetCurrentClipName()
	{
		var clipInfo = animator.GetCurrentAnimatorClipInfo(0).FirstOrDefault();
		return clipInfo.clip.name;
	}
}
