using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAttack : CharacterFrameAttackState
{
	[Header("위 세팅 값이 적용되는 캐릭터 타입")]
	[SerializeField] private ENUM_CHARACTER_TYPE attackCharacterType = ENUM_CHARACTER_TYPE.Red;
	
	protected override bool DoFrameAction()
	{
		if (characterType != attackCharacterType)
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
