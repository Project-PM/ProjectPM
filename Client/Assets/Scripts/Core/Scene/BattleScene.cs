using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemParam
{
    public readonly int playerId; 

    public SystemParam(int playerId)
    {
        this.playerId = playerId;
    }
}

public class BattleScene : MonoBehaviour
{
    [SerializeField] private SessionSystem sessionSystem;
    [SerializeField] private FrameInputSystem frameInputSystem;
    [SerializeField] private DebugSystem debugSystem;
    [SerializeField] private SpawnSystem spawnSystem;

    private SystemParam systemParam = new SystemParam(1000);

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
        sessionSystem.OnEnter(systemParam);
        frameInputSystem.OnEnter(systemParam);
        debugSystem.OnEnter(systemParam);
		spawnSystem.OnEnter(systemParam);
	}

    private void Update()
    {
        currentFrameCount++;
        currentDeltaTime += Time.deltaTime;

        sessionSystem.OnPrevUpdate(currentFrameCount, currentDeltaTime);
		frameInputSystem.OnPrevUpdate(currentFrameCount, currentDeltaTime);
        debugSystem.OnPrevUpdate(currentFrameCount, currentDeltaTime);
        spawnSystem.OnPrevUpdate(currentFrameCount, currentDeltaTime);

		sessionSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        frameInputSystem.OnUpdate(currentFrameCount, currentDeltaTime);
        debugSystem.OnUpdate(currentFrameCount, currentDeltaTime);
		spawnSystem.OnUpdate(currentFrameCount, currentDeltaTime);
	}

	private void LateUpdate()
	{
		frameInputSystem.OnLateUpdate(currentFrameCount, currentDeltaTime);
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
