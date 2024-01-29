using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMainWindow : UIMainWindow
{
	[SerializeField] private VariableJoystick joystick;

	protected override void Reset()
	{
		base.Reset();

		joystick = GetComponentInChildren<VariableJoystick>();
	}

}
