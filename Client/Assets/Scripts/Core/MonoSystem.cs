using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AssetLoadHelper
{
    private const string SystemAssetPathFormat = "Assets/Bundle/System/{0}Asset.asset";


	public static string GetSystemAssetPath<T>() where T : MonoSystem
    {
        return GetSystemAssetPath(typeof(T));
    }

    public static string GetSystemAssetPath(Type systemType)
    {
        return string.Format(SystemAssetPathFormat, systemType);
    }

    public static MonoSystem GetSystemAsset(Type systemType)
    {
        return AssetDatabase.LoadAssetAtPath<MonoSystem>(GetSystemAssetPath(systemType));
	}


	public static TSystem GetSystemAsset<TSystem>() where TSystem : MonoSystem
    {
        return AssetDatabase.LoadAssetAtPath<TSystem>(GetSystemAssetPath<TSystem>());
    }
}


public abstract class MonoSystem : ScriptableObject
{
	protected int playerId = -1;

	public virtual void OnEnter(SystemParam param)
    {
		playerId = param.playerId;
	}

	public virtual void OnExit()
    {
        playerId = -1;
	}

    public virtual void OnPrevUpdate(int deltaFrameCount, float deltaTime)
    {

    }

    public virtual void OnUpdate(int deltaFrameCount, float deltaTime)
    {
        
    }

    public virtual void OnLateUpdate(int deltaFrameCount, float deltaTime)
    {

    }
}
