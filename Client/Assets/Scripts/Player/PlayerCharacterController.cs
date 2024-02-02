using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
				// �������� ������ �Ǻ�
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
