using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoSystem
{
    [SerializeField] private bool useGravity = true;

	[SerializeField] private PlayerComponent playerComponentPrefab = null;

	private int playerId = -1;

	public override void OnEnter(SystemParam param)
	{
		playerId = param.playerId;
	}

    public void Spawn()
    {
		var playerComponent = FindObjectOfType<PlayerComponent>();
		if (playerComponent == null)
		{
			playerComponent = Instantiate(playerComponentPrefab);
		}

		playerComponent.SetPlayerInfo(playerId, ENUM_CHARACTER_TYPE.Red);

		var groundCheckComponent = FindObjectOfType<PlayerGroundCheckComponent>();
		if (groundCheckComponent == null)
			return;

		groundCheckComponent.UseGravity = useGravity;
	}
}
