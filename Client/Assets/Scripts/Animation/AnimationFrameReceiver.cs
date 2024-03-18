using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationFrameReceiver : MonoBehaviour
{
	[Header("애니메이터에서 수정되는 변수")]
	public int frameIndex = -1;

	public void Reset()
	{
		frameIndex = -1;
	}
}
