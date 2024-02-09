using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


public class FrameInputSystem : SyncSystem
{
	[SerializeField] private bool isOfflineMode = true;
	[SerializeField] private int targetFrameRate = 30;

	private Queue<FrameInputData> inputDataQueue = new Queue<FrameInputData>();
	private RES_FRAME_INPUT receiveFrameInput = new RES_FRAME_INPUT();

	public event Action<RES_FRAME_INPUT> onReceiveFrameInput = null;

	private int sendFrameNumber = -1;

	public override void OnEnter(SystemParam param)
	{
		base.OnEnter(param);

		sendFrameNumber = -1;
		Application.targetFrameRate = targetFrameRate;
	}

	public override void OnExit()
	{
		base.OnExit();
	}

	public void OnMoveInputChanged(Vector2 input)
	{
		var inputData = new MoveInputData(input, sendFrameNumber);
		inputDataQueue.Enqueue(inputData);
	}

	public void OnGuardInputChanged(bool isPress)
	{
		var inputData = new GuardInputData(isPress, sendFrameNumber);
		inputDataQueue.Enqueue(inputData);
	}

	public void OnAttackInputChanged(ENUM_ATTACK_KEY key, bool isAttack)
	{
		var inputData = new AttackInputData(key, isAttack, sendFrameNumber);
		inputDataQueue.Enqueue(inputData);
	}

    public void OnJumpInputChanged(bool isPress)
    {
        var inputData = new JumpInputData(isPress, sendFrameNumber);
        inputDataQueue.Enqueue(inputData);
    }

    public RES_FRAME_INPUT MakeFakePacket(int frameNumber)
	{
		var sendInput = MakeFrameInputPacket(frameNumber);
		if (sendInput == null)
			return null;

		var receiveFrameInput = new RES_FRAME_INPUT();
		receiveFrameInput.frameNumber = sendInput.frameNumber;

		var myPlayerInput = new RES_FRAME_INPUT.PlayerInput();
		myPlayerInput.playerId = playerId;
		myPlayerInput.moveX = sendInput.moveX;
		myPlayerInput.moveY = sendInput.moveY;
		myPlayerInput.attackKey = sendInput.attackKey;
		myPlayerInput.isJump = sendInput.isJump;
		myPlayerInput.isGuard = sendInput.isGuard;

		var otherPlayerInput = new RES_FRAME_INPUT.PlayerInput();
		otherPlayerInput.playerId = -1;

		receiveFrameInput.playerInputs = new List<RES_FRAME_INPUT.PlayerInput>
		{
			myPlayerInput,
            otherPlayerInput
        };

		return receiveFrameInput;
	}

	public override void OnPrevUpdate(int deltaFrameCount, float deltaTime)
	{
		base.OnPrevUpdate(deltaFrameCount, deltaTime);

#if UNITY_EDITOR
		if (isOfflineMode)
		{
			sendFrameNumber = receiveFrameInput.frameNumber + 1;

			receiveFrameInput = MakeFakePacket(sendFrameNumber);
			onReceiveFrameInput?.Invoke(receiveFrameInput);
		}
#endif
	}

	public override void OnLateUpdate(int deltaFrameCount, float deltaTime)
	{
		base.OnLateUpdate(deltaFrameCount, deltaTime);

		if (IsReceivedPacket())
		{
			SendPacket();
		}
	}

	private void SendPacket()
	{
		var sendInput = MakeFrameInputPacket(receiveFrameInput.frameNumber);
		if (sendInput == null)
			return;

		if (Send(sendInput) == false)
			return;

		sendFrameNumber = sendInput.frameNumber;
	}

	private bool IsReceivedPacket()
	{
		return isOfflineMode == false && receiveFrameInput.frameNumber == sendFrameNumber + 1;
	}

	public override void OnReceive(IPacket packet)
	{
		if (packet is RES_FRAME_INPUT frameInput)
		{
			receiveFrameInput = frameInput;
			onReceiveFrameInput?.Invoke(frameInput);
		}
	}

	private REQ_FRAME_INPUT MakeFrameInputPacket(int frameNumber)
	{
		Vector2 moveVec = Vector2.zero;
		ENUM_ATTACK_KEY pressedAttackKey = ENUM_ATTACK_KEY.NONE;
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

        var sendInput = new REQ_FRAME_INPUT();

		sendInput.playerId = playerId;
		sendInput.frameNumber = frameNumber;
		sendInput.moveX = moveVec.x;
		sendInput.moveY = moveVec.y;
		sendInput.attackKey = (int)pressedAttackKey;
		sendInput.isGuard = isGuard;
		sendInput.isJump = isJump;

		return sendInput;
    }
}
