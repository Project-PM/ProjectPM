using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	private int playerId = -1;
	private ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.None;

	public void SetPlayerId(int playerId)
	{
		this.playerId = playerId;
	}

	public void SetCharacter(ENUM_CHARACTER_TYPE characterType)
	{
		this.characterType = characterType;
	}

	private void Start()
	{
		CharacterControllerState.Initialize(characterType, animator);
	}

	private void OnEnable()
	{
		System.onReceiveFrameInput += OnReceiveFrameInput;
	}

	private void OnDisable()
	{
		System.onReceiveFrameInput -= OnReceiveFrameInput;
	}

	private void OnReceiveFrameInput(RES_FRAME_INPUT frameInput)
	{
		RES_FRAME_INPUT.PlayerInput playerInput = frameInput.playerInputs.FirstOrDefault(i => i.playerId == playerId);
		
		CheckFall(playerInput);
		CheckGrounded(playerInput);
		CheckMove(playerInput);
		CheckJump(playerInput);
		CheckAttack(playerInput);
	}

	private void CheckGrounded(RES_FRAME_INPUT.PlayerInput playerInput)
	{
		bool isGrounded = IsGrounded();
		if (isGrounded)
		{
			
		}
	}

	private void CheckMove(RES_FRAME_INPUT.PlayerInput playerInput)
	{
		onMove?.Invoke(new Vector2(playerInput.moveX, 0));
	}

	private void CheckJump(RES_FRAME_INPUT.PlayerInput playerInput)
	{
		if (playerInput.isJump)
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

	private void CheckFall(RES_FRAME_INPUT.PlayerInput playerInput)
	{
		if (IsGrounded() == false)
		{
			if (IsFallTimeout())
			{
				// 떨어지는 중임을 판별
			}
		}
	}

	private void CheckAttack(RES_FRAME_INPUT.PlayerInput playerInput)
	{
		if (playerInput.attackKey == (int)ENUM_ATTACK_KEY.ATTACK)
		{
			animator.Play("Attack1");
		}
	}

}
