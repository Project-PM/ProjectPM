using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum ENUM_ATTACK_KEY
{
	NONE = 0, // ���� ����
	ATTACK, // �Ϲ� ����
	SKILL, // ��ų
	MAX 
}

public class FrameInputData
{
	public readonly int frameCount;

	public FrameInputData(int frameCount)
	{
		this.frameCount = frameCount;
	}	
}

public class MoveInputData : FrameInputData
{
	public readonly Vector2 moveVector = Vector2.zero; // �����¿� �̵�

	public MoveInputData() : base(0)
	{
		this.moveVector = default;
	}

	public MoveInputData(Vector2 moveVector, int frameCount) : base(frameCount)
	{
		this.moveVector = moveVector;
	}
}

public class AttackInputData : FrameInputData
{
	public readonly ENUM_ATTACK_KEY key;
	public readonly bool isPress = false;

	public AttackInputData(ENUM_ATTACK_KEY key, bool isPress, int frameCount) : base(frameCount)
	{
		this.key = key;
		this.isPress = isPress;
	}
}

public abstract class PressInputData : FrameInputData
{
	public readonly bool isPress = false; 

	public PressInputData(bool isPress, int frameCount) : base(frameCount)
	{
		this.isPress = isPress;
	}
}

public class DashInputData : PressInputData
{
	public DashInputData(bool isPress, int frameCount) : base(isPress, frameCount)
	{
	}
}

public class REQ_FRAME_INPUT
{
	public readonly Vector2 moveVec;
	public readonly ENUM_ATTACK_KEY pressedAttackKey;
	public readonly bool isDash;

	public REQ_FRAME_INPUT(Vector2 moveVec, ENUM_ATTACK_KEY pressedAttackKey, bool isDash, int targetFrameCount) 
	{
		this.moveVec = moveVec;
		this.pressedAttackKey = pressedAttackKey;
		this.isDash = isDash;
	}
}

public class FrameInputSystem : MonoSystem
{
	public static float MoveThreshold { get; private set; } = 1;
    public static JoystickType JoystickType { get; private set; } = JoystickType.Fixed;
    public static float HandleRange { get; private set; } = 1;
    public static float DeadZone { get; private set; } = 0;
    public static AxisOptions AxisOptions { get; private set; } = AxisOptions.Both;
    public static bool SnapX { get; private set; } = true;
	public static bool SnapY { get; private set; } = true;

	private Queue<FrameInputData> inputDataQueue = new Queue<FrameInputData>();

	public override void OnEnter()
	{
		SetJoystick();
	}

	private void SetJoystick()
	{
		var joyStick = UnityEngine.Object.FindObjectOfType<VariableJoystick>();
		if (joyStick != null)
		{
			joyStick.SetMode(JoystickType);
		}
	}

	public override void OnExit()
	{
		
	}

	public void OnMoveInputChanged(Vector2 input, int frameCount)
	{
		float x = SnapX ? SnapFloat(input, input.x, AxisOptions.Horizontal) : input.x;
		float y = SnapY ? SnapFloat(input, input.y, AxisOptions.Vertical) : input.y;

		var inputData = new MoveInputData(new Vector2(x, y), frameCount);
		inputDataQueue.Enqueue(inputData);
	}

	public void OnDashInputChanged(bool isPress, int frameCount)
	{
		var inputData = new DashInputData(isPress, frameCount);
		inputDataQueue.Enqueue(inputData);
	}

	public void OnAttackInputChanged(ENUM_ATTACK_KEY key, bool isAttack, int frameCount)
	{
		var inputData = new AttackInputData(key, isAttack, frameCount);
		inputDataQueue.Enqueue(inputData);
	}

	/// <summary>
	/// ���⸦ �ٷ� REQ�� ������.
	/// RES�� ���� Output �����͸� ĳ���Ϳ��� �����ϴ�.
	/// </summary>
	/// <param name="targetFrameCount"></param>
	/// <returns></returns>
	/// 
	public REQ_FRAME_INPUT MakeFakePacket()
	{
		return MakeFrameInputPacket(Time.frameCount);
	}

	private REQ_FRAME_INPUT MakeFrameInputPacket(int targetFrameCount)
	{
		Vector2 moveVec = Vector2.zero;
		ENUM_ATTACK_KEY pressedAttackKey = ENUM_ATTACK_KEY.MAX;
		bool isDash = false;

		while (inputDataQueue.TryDequeue(out var result))
		{
			if (result is MoveInputData moveInputResult)
			{
				moveVec = moveInputResult.moveVector;
			}
			else if (result is AttackInputData attackInputResult)
			{
				if (attackInputResult.isPress)
				{
					pressedAttackKey = attackInputResult.key;
				}
			}
			else if (result is PressInputData jumpInputResult)
			{
				isDash = jumpInputResult.isPress;
			}
		}

		return new REQ_FRAME_INPUT(moveVec, pressedAttackKey, isDash, targetFrameCount);
	}

	private float SnapFloat(Vector2 input, float value, AxisOptions snapAxis)
	{
		if (value == 0)
			return value;

		if (AxisOptions == AxisOptions.Both)
		{
			float angle = Vector2.Angle(input, Vector2.up);
			if (snapAxis == AxisOptions.Horizontal)
			{
				if (angle < 22.5f || angle > 157.5f)
				{
					return 0;
				}
				else
				{
					return (value > 0) ? 1 : -1;
				}
			}
			else if (snapAxis == AxisOptions.Vertical)
			{
				if (angle > 67.5f && angle < 112.5f)
				{
					return 0;
				}
				else
				{
					return (value > 0) ? 1 : -1;
				}
			}
			else
			{
				return value;
			}
		}
		else
		{
			if (value > 0)
			{
				return 1;
			}
			else if (value < 0)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}
	}
}
