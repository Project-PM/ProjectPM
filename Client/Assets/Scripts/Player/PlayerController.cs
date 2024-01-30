using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 1. ��ǲ�� ������ üũ�Ѵ�.
// 2. ������Ʈ�� �Ŵ޾� ���� Func��� ���¸� üũ�Ѵ�.
// 3. ������ ������Ʈ�� �ִϸ��̼��� �����Ѵ�.
// 4. ������Ʈ���� ������ �Լ��� ��� �����Ų��.
// 5. �Լ��� ������Ʈ�鿡�� Action�� �Ѹ���.

public class PlayerController : MonoComponent<FrameInputSystem>
{
	[SerializeField] private Animator animator;

	public event Func<bool> IsNotYetJump;
	public event Func<bool> IsFallTimeout;
	public event Func<bool> IsGrounded;

	public event Action onJump = null;
	public event Action<Vector2> onMove = null;
	public event Action onAttack = null;

	private int playerId = -1;

	private REQ_FRAME_INPUT currentFrameInput = null;

	public void SetPlayerId(int playerId)
	{
		this.playerId = playerId;
	}

	private void Awake()
	{
		// ������Ʈ �ӽ� Ȱ��ȭ
		PlayerControllerState.Initialize(animator);
	}

	private void Update()
	{
        currentFrameInput = System.MakeFakePacket();

		CheckFall();
		CheckGrounded();
		CheckMove();
		CheckJump();

    }

	private void CheckGrounded()
	{
		bool isGrounded = IsGrounded();
		if (isGrounded)
		{
			
		}
	}

	private void CheckMove()
	{
		onMove?.Invoke(new Vector2(currentFrameInput.moveVec.x, 0));
	}

	private void CheckJump()
	{
		if (currentFrameInput.isJump)
		{
            if (IsGrounded())
            {
                if (IsNotYetJump())
                {
                    onJump?.Invoke();
                }
            }
        }
	}


	private void CheckFall()
	{
		if (IsGrounded() == false)
		{
			if (IsFallTimeout())
			{
				// �������� ������ �Ǻ�
			}
		}
	}

}
