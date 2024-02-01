using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(RedManAttack1))]
[CanEditMultipleObjects]
public class RedManAttack1Drawer : PlayerFrameAttackDrawer { }

#endif

public class RedManAttack1 : PlayerFrameAttackState
{
	protected override bool DoFrameAction()
	{
		if (characterType != ENUM_CHARACTER_TYPE.Red)
			return false;

		var hitObjects = GetDamageableObjects();
		if (hitObjects.Any() == false)
			return false;

		bool isSuccess = false;

		foreach (var hitObject in hitObjects)
		{
			isSuccess |= hitObject.OnHit(1);
		}

		return isSuccess;
	}
}
