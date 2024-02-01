using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 1. 인풋을 꺼내어 체크한다.
// 2. 컴포넌트에 매달아 놓은 Func들로 상태를 체크한다.
// 3. 적절한 스테이트의 애니메이션을 수행한다.
// 4. 스테이트에서 적당한 함수를 골라 수행시킨다.
// 5. 함수는 컴포넌트들에게 Action을 뿌린다.

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
				// 떨어지는 중임을 판별
			}
		}
	}

}
