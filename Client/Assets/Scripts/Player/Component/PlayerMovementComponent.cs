using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : PlayerChildComponent
{
	[SerializeField] private Rigidbody2D rigidBody;
	[SerializeField] private PlayerGroundCheckComponent groundCheckComponent;

	private void OnEnable()
	{
		controller.onMove += OnMove;
		controller.onMoveInput += OnMoveInput;
	}

	private void OnDisable()
	{
		controller.onMove -= OnMove;
        controller.onMoveInput -= OnMoveInput;
    }

	private void OnMove(Vector2 moveVec)
	{
        rigidBody.position = rigidBody.position + moveVec;
	}

	private void OnMoveInput(float moveX)
	{
        Vector2 finalMoveVec = new Vector2(moveX, groundCheckComponent._verticalVelocity);
        rigidBody.position = rigidBody.position + finalMoveVec;
    }
}
