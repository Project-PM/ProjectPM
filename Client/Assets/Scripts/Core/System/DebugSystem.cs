using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoSystem
{
    private PlayerComponent playerComponent = null;

    public override void OnEnter()
    {
        base.OnEnter();

		playerComponent = FindObjectOfType<PlayerComponent>();
		playerComponent.SetPlayerId(1000);
    }
}
