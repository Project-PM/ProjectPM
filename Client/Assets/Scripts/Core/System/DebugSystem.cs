using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoSystem
{
    [SerializeField] private bool isEnable = false;
	[SerializeField] private PlayerComponent playerComponentPrefab = null;

    public override void OnEnter()
    {
        base.OnEnter();

        if (isEnable)
        {
			var playerComponent = Instantiate(playerComponentPrefab);
			playerComponent.SetPlayerId(1000);
            playerComponent.SetInput(true);
		}
    }
}
