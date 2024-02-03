using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrontCheckComponent : MonoBehaviour
{
	[SerializeField] private PlayerCharacterController controller;

	private void OnEnable()
	{
		controller.IsFrontRight += IsFrontRight;
	}

	private void OnDisable()
	{
		controller.IsFrontRight -= IsFrontRight;
	}

	private bool IsFrontRight()
	{
		return true;
	}
}