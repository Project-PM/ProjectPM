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

public enum DamageType
{
	Airborne,
	Stand,
	Down
}

public class PlayerCharacterController : MonoComponent<FrameInputSystem>
{
	[SerializeField] private Animator animator;

	public event Func<bool> IsNotYetJump;
	public event Func<bool> IsFallTimeout;
	public event Func<bool> IsGrounded;
	public event Func<bool> IsFrontRight;

	public event Action<float> onJump = null;
	public event Action<Vector2> onMove = null;
	public event Action onAttack = null;

	private int playerId = -1;
	private ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.None;

	private RES_FRAME_INPUT.PlayerInput myPlayerInput = new RES_FRAME_INPUT.PlayerInput();

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
		myPlayerInput = frameInput.playerInputs.FirstOrDefault(i => i.playerId == playerId);
	}

	public bool CheckMove(out bool isFront)
	{
		isFront = false;

		if (IsGrounded() == false)
			return false;

		if (myPlayerInput == null)
			return false;

		if (myPlayerInput.moveX < 0.01f && myPlayerInput.moveX > -0.01f)
			return false;

		isFront = IsFrontRight() && myPlayerInput.moveX > 0.01f;
		isFront |= IsFrontRight() == false && myPlayerInput.moveX < 0.01f;

		return true;
	}

	public bool CheckJumpable()
	{
		if (myPlayerInput == null)
			return false;

		if (myPlayerInput.isJump == false)
			return false;

		if (IsGrounded() == false)
			return false;

		if (IsNotYetJump() == false)
			return false;

		return true;
	}

	public bool CheckGrounded()
	{
		return IsGrounded();
	}

	public bool CheckFall()
	{
		if (IsGrounded())
			return false;

		if (IsFallTimeout() == false)
			return false;

		return true;
	}

	public bool CheckAttack()
	{
		if (myPlayerInput == null)
			return false;

		return myPlayerInput.attackKey == (int)ENUM_ATTACK_KEY.ATTACK;
	}

	public bool CheckSkill()
	{
		if (myPlayerInput == null)
			return false;

		return myPlayerInput.attackKey == (int)ENUM_ATTACK_KEY.SKILL;
	}

	public bool CheckUltimate()
	{
		if (myPlayerInput == null)
			return false;

		return myPlayerInput.attackKey == (int)ENUM_ATTACK_KEY.ULTIMATE;
	}

	public bool CheckHit(out DamageType damageType)
	{
		damageType = DamageType.Stand;
		return false;
	}

	public void TryMove(float moveSpeed)
	{
		if (myPlayerInput == null)
			return;

		Vector2 moveVec = new Vector2(myPlayerInput.moveX * moveSpeed, 0);
		onMove?.Invoke(moveVec);
	}

	public void TryJump(float jumpHeight)
	{
		onJump?.Invoke(jumpHeight);
	}
}
