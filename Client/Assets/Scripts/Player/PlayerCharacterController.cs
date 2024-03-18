using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerCharacterController : MonoComponent<FrameInputSystem>
{
	[SerializeField] private Animator animator;

	public event Func<bool> IsNotYetJump;
	public event Func<bool> IsFallTimeout;
	public event Func<bool> IsGrounded;
	public event Func<bool> IsFrontRight;
	public event Func<bool> IsSuccessAttack;
	public event Func<float> OnGetHeightFromGround;

	public Func<ENUM_DAMAGE_TYPE> IsHit;
	public event Func<DamageInfo> GetDamageInfo;

	public event Action<float> onJump = null;
	public event Action<Vector2> onMove = null;
	public event Action<float> onMoveInput = null;
	public event Action<bool> onSetGravity = null;

	public int playerId { get; private set; }
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
        animator.InitializeCharacter(characterType);
	}

	private void LateUpdate()
	{
		ENUM_DAMAGE_TYPE damageType = IsHit();
		bool isSuccessAttack = IsSuccessAttack();

		System.OnPlayerInputChanged(playerId, damageType, isSuccessAttack);
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

	public bool CheckFrontRight()
	{
		return IsFrontRight();
    }

	public DamageInfo TryGetDamageInfo()
	{
		return GetDamageInfo();
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

	public float GetHeightFromGround()
	{
		return OnGetHeightFromGround();
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

	public bool CheckSuccessAttack()
	{
		if (myPlayerInput == null)
			return false;

		return myPlayerInput.isSuccessAttack;
	}

	public bool CheckSkill()
	{
		if (myPlayerInput == null)
			return false;

		return myPlayerInput.attackKey == (int)ENUM_ATTACK_KEY.SKILL;
	}

	public bool CheckGuard()
	{
        if (myPlayerInput == null)
            return false;

		return myPlayerInput.isGuard;
    }

	public bool CheckUltimate()
	{
		if (myPlayerInput == null)
			return false;

		return myPlayerInput.attackKey == (int)ENUM_ATTACK_KEY.ULTIMATE;
	}

	public bool CheckHit(out ENUM_DAMAGE_TYPE damageType)
	{
		damageType = ENUM_DAMAGE_TYPE.None;
		if (myPlayerInput == null)
			return false;

		damageType = (ENUM_DAMAGE_TYPE)myPlayerInput.damageType;
		return damageType != ENUM_DAMAGE_TYPE.None;
	}

	public void TryMoveAndJump(float moveSpeed = 0.0f)
	{
		if (myPlayerInput == null)
			return;

        onMoveInput?.Invoke(myPlayerInput.moveX * moveSpeed);
	}

	public void MovePosition(float moveX, float moveY)
	{
		MovePosition(new Vector2(moveX, moveY));
	}

	public void MovePosition(Vector2 moveVec)
	{
        onMove?.Invoke(moveVec);
    }

    public void TryJump(float jumpHeight)
	{
		onJump?.Invoke(jumpHeight);
	}

	public void TrySetGravity(bool useGravity)
	{
        onSetGravity?.Invoke(useGravity);
    }	
}
