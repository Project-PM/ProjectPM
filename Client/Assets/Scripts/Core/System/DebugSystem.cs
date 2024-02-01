using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoSystem
{
    [SerializeField] private bool isEnable = false;
    [SerializeField] private bool isUsableInput = false;
    [SerializeField] private bool useGravity = true;

	[SerializeField] private PlayerComponent playerComponentPrefab = null;

    public override void OnEnter()
    {
        base.OnEnter();

        if (isEnable)
        {
            var playerComponent = FindObjectOfType<PlayerComponent>();
            if (playerComponent == null)
            {
				playerComponent = Instantiate(playerComponentPrefab);
			}

			playerComponent.SetPlayerInfo(1000, ENUM_CHARACTER_TYPE.Red);
            playerComponent.SetInput(isUsableInput);

            var groundCheckComponent = FindObjectOfType<PlayerGroundCheckComponent>();
            if (groundCheckComponent == null)
                return;

            groundCheckComponent.UseGravity = useGravity;
		}
    }
}
