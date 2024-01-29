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

	public void SetPlayerId(int playerId)
	{
		this.playerId = playerId;
	}

	private void Awake()
	{
		// 스테이트 머신 활성화
		PlayerControllerState.Initialize(animator);
	}

	private void Update()
	{
		CheckInput();
		CheckFall();
		CheckGrounded();
		CheckMove();
	}

	private void CheckInput()
	{
		var input = system.MakeFakePacket();
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
		onMove?.Invoke(Vector2.zero);
	}

	private void OnJump()
	{
		if (IsGrounded())
		{
			if (IsNotYetJump())
			{
				onJump?.Invoke();
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
