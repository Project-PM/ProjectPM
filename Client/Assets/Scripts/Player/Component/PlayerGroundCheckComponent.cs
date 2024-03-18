using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGroundCheckComponent : PlayerChildComponent
{
	[SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask GroundLayers;

    [SerializeField] private float GroundedRadius = 0.35f;
	[SerializeField] private float Gravity = -0.1f;

	private float _jumpTimeoutDelta = 0.0f;
	private float _fallTimeoutDelta = 0.0f;

	private float _terminalVelocity = 1.0f;

	private float JumpTimeout = 0.50f;
	private float FallTimeout = 0.15f;
	private float GroundedOffset = 0.0f;

	private bool isFallTimeout = false;

	public float _verticalVelocity { get; private set; }

	public bool UseGravity = true;

	private void OnEnable()
	{
		GroundedOffset = boxCollider.offset.y - boxCollider.size.y / 2;

		controller.IsGrounded += IsGrounded;
		controller.IsFallTimeout += IsFallTimeout;
		controller.IsNotYetJump += IsFirstJumpTrigger;
		controller.onJump += OnJump;
		controller.OnGetHeightFromGround += GetHeightFromGround;
	}

	private void OnDisable()
	{
		controller.IsGrounded -= IsGrounded;
		controller.IsFallTimeout -= IsFallTimeout;
		controller.IsNotYetJump -= IsFirstJumpTrigger;
		controller.onJump -= OnJump;
		controller.OnGetHeightFromGround -= GetHeightFromGround;
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
            _verticalVelocity = 0.0f;
        }
    }

    private bool IsFallTimeout()
	{
		return isFallTimeout;
	}

	private void LateUpdate()
	{
		if (UseGravity)
		{
			JumpAndGravity();
		}
	}

	private bool IsGrounded()
	{
		Vector2 startPos = new Vector2(transform.position.x, transform.position.y + GroundedOffset);

		Debug.DrawRay(startPos, Vector2.down * GroundedRadius);
		return Physics2D.Raycast(startPos, Vector2.down, GroundedRadius, GroundLayers);
	}

	private float GetHeightFromGround()
	{
		Vector2 startPos = new Vector2(transform.position.x, transform.position.y + GroundedOffset);

		RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, Vector2.down, 1000.0f, GroundLayers);
		if (hits.Any() == false)
			return 0.0f;

		RaycastHit2D groundHit = hits.FirstOrDefault();
		return groundHit.distance;
	}

	private bool IsFirstJumpTrigger()
	{
		return _jumpTimeoutDelta <= 0.0f;
	}

	private void OnJump(float jumpHeight)
	{
		_verticalVelocity = Mathf.Sqrt(-1f * jumpHeight * Gravity);
	}

	private void JumpAndGravity()
	{
		isFallTimeout = false;

		if (IsGrounded())
		{
			_fallTimeoutDelta = FallTimeout;

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

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
	}
}
