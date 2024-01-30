using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    [SerializeField] private SessionSystem sessionSystem;
    [SerializeField] private SyncSystem syncSystem;
    [SerializeField] private FrameInputSystem frameInputSystem;

    private int currentFrameCount = 0;
    private float currentDeltaTime = 0;

    private void Start()
    {
        sessionSystem.OnEnter();
        syncSystem.OnEnter();
        frameInputSystem.OnEnter();
    }

    private void Update()
    {
        currentFrameCount++;
        currentDeltaTime += Time.deltaTime;

        syncSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        sessionSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        frameInputSystem.OnUpdate(currentFrameCount, currentDeltaTime);
    }

    private void OnDestroy()
    {
        sessionSystem.OnExit();
        syncSystem.OnExit();
        frameInputSystem.OnExit();
    }
}
