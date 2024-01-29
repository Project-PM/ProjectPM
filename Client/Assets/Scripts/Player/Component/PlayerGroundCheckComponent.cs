using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheckComponent : MonoBehaviour
{
	[SerializeField] private PlayerController controller;

	[SerializeField] private BoxCollider2D boxCollider;
	[SerializeField] private float GroundedRadius = 0.28f;

	[SerializeField] private LayerMask GroundLayers;
	[SerializeField] private float Gravity = -0.01f;
	[SerializeField] private float JumpHeight = 1.2f;

	private float _jumpTimeoutDelta = 0.0f;
	private float _fallTimeoutDelta = 0.0f;

	private float _terminalVelocity = 1.0f;

	

	private float JumpTimeout = 0.50f;
	private float FallTimeout = 0.15f;
	private float GroundedOffset = 0.0f;

	private bool isFallTimeout;


	public float _verticalVelocity { get; private set; }

	private void OnEnable()
	{
		GroundedOffset = boxCollider.size.y / 2;

		controller.IsGrounded += IsGrounded;
		controller.IsFallTimeout += IsFallTimeout;
		controller.IsNotYetJump += IsFirstJumpTrigger;
		controller.onJump += OnJump;
	}

	private void OnDisable()
	{
		controller.IsGrounded -= IsGrounded;
		controller.IsFallTimeout -= IsFallTimeout;
		controller.IsNotYetJump -= IsFirstJumpTrigger;
		controller.onJump -= OnJump;
	}

	private bool IsFallTimeout()
	{
		return isFallTimeout;
	}

	private void Update()
	{
		JumpAndGravity();
	}

	private bool IsGrounded()
	{
		Vector2 startPos = new Vector2(transform.position.x, transform.position.y - GroundedOffset);

		Debug.DrawRay(startPos, Vector2.down * GroundedRadius);
		return Physics2D.Raycast(startPos, Vector2.down, GroundedRadius);
	}

	private bool IsFirstJumpTrigger()
	{
		return _jumpTimeoutDelta <= 0.0f;
	}

	private void OnJump()
	{
		_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
	}

	private void JumpAndGravity()
	{
		isFallTimeout = false;

		if (IsGrounded())
		{
			_fallTimeoutDelta = FallTimeout;

			if (_verticalVelocity < 0.0f)
			{
				_verticalVelocity = -0.01f;
			}

			if (_jumpTimeoutDelta >= 0.0f)
			{
				_jumpTimeoutDelta -= Time.deltaTime;
			}
		}
		else
		{
			_jumpTimeoutDelta = JumpTimeout;

			if (_fallTimeoutDelta >= 0.0f)
			{
				_fallTimeoutDelta -= Time.deltaTime;
			}
			else
			{
				isFallTimeout = true;
			}
		}

		if (_verticalVelocity < _terminalVelocity)
		{
			_verticalVelocity += Gravity * Time.deltaTime;
		}
	}
}
