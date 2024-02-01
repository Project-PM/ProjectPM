using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScene : MonoBehaviour
{
	[SerializeField] private DebugSystem debugSystem;

	public event Action onDrawGizmos = null;

	private int currentFrameCount = 0;
	private float currentDeltaTime = 0;

	private void Reset()
	{
		debugSystem = AssetLoadHelper.GetSystemAsset<DebugSystem>();
	}

	private void Start()
	{
		debugSystem.OnEnter();
	}

	private void Update()
	{
		currentFrameCount++;
		currentDeltaTime += Time.deltaTime;

		debugSystem.OnUpdate(currentFrameCount, currentDeltaTime);
	}

	private void OnDestroy()
	{
		debugSystem.OnExit();
	}


	private void OnDrawGizmos()
	{
		onDrawGizmos?.Invoke();
	}
}
