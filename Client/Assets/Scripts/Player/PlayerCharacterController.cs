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

public class PlayerCharacterController : MonoComponent<FrameInputSystem>
{
	[SerializeField] private Animator animator;

	public event Func<bool> IsNotYetJump;
	public event Func<bool> IsFallTimeout;
	public event Func<bool> IsGrounded;

	public event Action onJump = null;
	public event Action<Vector2> onMove = null;
	public event Action onAttack = null;

	private bool isEnable = false;
	private ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.None;

	private REQ_FRAME_INPUT currentFrameInput = null;

	public void SetInput(bool isEnable)
	{
		this.isEnable = isEnable;
	}

	public void SetCharacter(ENUM_CHARACTER_TYPE characterType)
	{
		this.characterType = characterType;
	}

	private void Start()
	{
		CharacterControllerState.Initialize(characterType, animator);
	}

	private void Update()
	{
		if (isEnable)
		{
			currentFrameInput = System.MakeFakePacket();
		}

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
		if (currentFrameInput == null)
			return;

		onMove?.Invoke(new Vector2(currentFrameInput.moveVec.x, 0));
	}

	private void CheckJump()
	{
		if (currentFrameInput == null)
			return;

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
