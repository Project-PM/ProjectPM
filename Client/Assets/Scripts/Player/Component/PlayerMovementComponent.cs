using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rigidBody;
	[SerializeField] private PlayerCharacterController controller;
	[SerializeField] private PlayerGroundCheckComponent groundCheckComponent;

	private void OnEnable()
	{
		controller.onMove += OnMove;
		controller.onMoveX += OnMoveX;
	}

	private void OnDisable()
	{
		controller.onMove -= OnMove;
        controller.onMoveX -= OnMoveX;
    }

	private void OnMove(Vector2 moveVec)
	{
		rigidBody.MovePosition(rigidBody.position + moveVec);
	}

	private void OnMoveX(float moveX)
	{
        Vector2 finalMoveVec = new Vector2(moveX, groundCheckComponent._verticalVelocity);
        rigidBody.MovePosition(rigidBody.position + finalMoveVec);
    }
}
