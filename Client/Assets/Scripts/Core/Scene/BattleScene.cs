using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    [SerializeField] private SessionSystem sessionSystem;
    [SerializeField] private SyncSystem syncSystem;
    [SerializeField] private FrameInputSystem frameInputSystem;
    [SerializeField] private DebugSystem debugSystem;
    [SerializeField] private SpawnSystem spawnSystem;

    private int currentFrameCount = 0;
    private float currentDeltaTime = 0;

    private void Reset()
    {
        sessionSystem = AssetLoadHelper.GetSystemAsset<SessionSystem>();
        syncSystem = AssetLoadHelper.GetSystemAsset<SyncSystem>();
        frameInputSystem = AssetLoadHelper.GetSystemAsset<FrameInputSystem>();
        debugSystem = AssetLoadHelper.GetSystemAsset<DebugSystem>();
		spawnSystem = AssetLoadHelper.GetSystemAsset<SpawnSystem>();

	}

    private void Start()
    {
        sessionSystem.OnEnter();
        syncSystem.OnEnter();
        frameInputSystem.OnEnter();
        debugSystem.OnEnter();
		spawnSystem.OnEnter();

	}

    private void Update()
    {
        currentFrameCount++;
        currentDeltaTime += Time.deltaTime;

        syncSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        sessionSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        frameInputSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        debugSystem.OnUpdate(currentFrameCount, currentDeltaTime);
		spawnSystem.OnUpdate(currentFrameCount, currentDeltaTime);

	}

    private void OnDestroy()
    {
        sessionSystem.OnExit();
        syncSystem.OnExit();
        frameInputSystem.OnExit();
        debugSystem.OnExit();
        spawnSystem.OnExit();
    }
}
