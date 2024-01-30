using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rigidBody;
	[SerializeField] private PlayerController controller;
	[SerializeField] private PlayerGroundCheckComponent groundCheckComponent;

	[SerializeField] private float moveSpeed;

	private void OnEnable()
	{
		controller.onMove += OnMove;
	}

	private void OnDisable()
	{
		controller.onMove -= OnMove;
	}

	private void OnMove(Vector2 moveVec)
	{
		Vector2 finalMoveVec = new Vector2(moveVec.x * moveSpeed, groundCheckComponent._verticalVelocity);
		Debug.Log(finalMoveVec);
		rigidBody.MovePosition(rigidBody.position + finalMoveVec);
	}
}
