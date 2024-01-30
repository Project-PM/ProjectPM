using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum ENUM_ATTACK_KEY
{
	NONE = 0, // 공격 안함
	ATTACK, // 일반 공격
	SKILL, // 스킬
	ULTIMATE, // 궁극기
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
	public readonly Vector2 moveVector = Vector2.zero; // 전후좌우 이동

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

public class GuardInputData : PressInputData
{
	public GuardInputData(bool isPress, int frameCount) : base(isPress, frameCount)
	{
	}
}

public class JumpInputData : PressInputData
{
    public JumpInputData(bool isPress, int frameCount) : base(isPress, frameCount)
    {
    }
}


public class REQ_FRAME_INPUT
{
	public readonly Vector2 moveVec;
	public readonly ENUM_ATTACK_KEY pressedAttackKey;
	public readonly bool isJump;
	public readonly bool isGuard;

	public REQ_FRAME_INPUT(Vector2 moveVec, ENUM_ATTACK_KEY pressedAttackKey, bool isJump, bool isGuard, int targetFrameCount) 
	{
		this.moveVec = moveVec;
		this.pressedAttackKey = pressedAttackKey;
		this.isJump = isJump;
		this.isGuard = isGuard;
	}
}

public class FrameInputSystem : MonoSystem
{
	public float MoveThreshold { get; private set; } = 1;
    public JoystickType JoystickType { get; private set; } = JoystickType.Fixed;
    public float HandleRange { get; private set; } = 1;
    public float DeadZone { get; private set; } = 0;
    public AxisOptions AxisOptions { get; private set; } = AxisOptions.Both;
    public bool SnapX { get; private set; } = true;
	public bool SnapY { get; private set; } = true;

	private Queue<FrameInputData> inputDataQueue = new Queue<FrameInputData>();
	private REQ_FRAME_INPUT currentFrameInput = null;

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

	public void OnGuardInputChanged(bool isPress, int frameCount)
	{
		var inputData = new GuardInputData(isPress, frameCount);
		inputDataQueue.Enqueue(inputData);
	}

	public void OnAttackInputChanged(ENUM_ATTACK_KEY key, bool isAttack, int frameCount)
	{
		var inputData = new AttackInputData(key, isAttack, frameCount);
		inputDataQueue.Enqueue(inputData);
	}

    public void OnJumpInputChanged(bool isPress, int frameCount)
    {
        var inputData = new JumpInputData(isPress, frameCount);
        inputDataQueue.Enqueue(inputData);
    }

    /// <summary>
    /// 여기를 바로 REQ로 보낸다.
    /// RES로 받은 Output 데이터를 캐릭터에게 보냅니다.
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
		Vector2 moveVec = currentFrameInput != null ? currentFrameInput.moveVec : Vector2.zero;
		ENUM_ATTACK_KEY pressedAttackKey = ENUM_ATTACK_KEY.MAX;
		bool isJump = false;
		bool isGuard = false;

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
			else if (result is JumpInputData jumpInputResult)
			{
				isJump = jumpInputResult.isPress;
			}
			else if(result is GuardInputData guardInputResult)
			{
				isGuard = guardInputResult.isPress;
			}
		}

        currentFrameInput = new REQ_FRAME_INPUT(moveVec, pressedAttackKey, isJump, isGuard, targetFrameCount);
		return currentFrameInput;

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
