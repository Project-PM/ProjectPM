using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    [SerializeField] private SessionSystem sessionSystem;
    [SerializeField] private FrameInputSystem frameInputSystem;
    [SerializeField] private DebugSystem debugSystem;
    [SerializeField] private SpawnSystem spawnSystem;

	private int currentFrameCount = 0;
    private float currentDeltaTime = 0;

    private void Reset()
    {
        sessionSystem = AssetLoadHelper.GetSystemAsset<SessionSystem>();
        frameInputSystem = AssetLoadHelper.GetSystemAsset<FrameInputSystem>();
        debugSystem = AssetLoadHelper.GetSystemAsset<DebugSystem>();
		spawnSystem = AssetLoadHelper.GetSystemAsset<SpawnSystem>();

	}

    private void Start()
    {
        sessionSystem.OnEnter();
        frameInputSystem.OnEnter();
        debugSystem.OnEnter();
		spawnSystem.OnEnter();

	}

    private void Update()
    {
        currentFrameCount++;
        currentDeltaTime += Time.deltaTime;

        sessionSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        frameInputSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        debugSystem.OnUpdate(currentFrameCount, currentDeltaTime);
		spawnSystem.OnUpdate(currentFrameCount, currentDeltaTime);
	}

    private void OnDestroy()
    {
        sessionSystem.OnExit();
        frameInputSystem.OnExit();
        debugSystem.OnExit();
        spawnSystem.OnExit();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 300, 150), "온라인 캐릭터 스폰"))
        {
            spawnSystem.Spawn();
        }
        else if (GUI.Button(new Rect(400, 50, 300, 150), "오프라인 캐릭터 스폰"))
        {
            debugSystem.Spawn();
        }
	}
}
