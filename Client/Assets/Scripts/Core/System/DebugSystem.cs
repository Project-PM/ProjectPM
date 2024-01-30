using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoSystem
{
    [SerializeField] private PlayerComponent playerComponent = null;

    protected override void OnReset()
    {
        playerComponent = FindObjectOfType<PlayerComponent>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        playerComponent.Initialize(1000);
    }
}
