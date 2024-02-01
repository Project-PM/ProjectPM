using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
