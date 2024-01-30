using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoComponent<TSystem> : MonoBehaviour where TSystem : MonoSystem
{
	private TSystem system = null;

    protected TSystem System
	{
		get
		{

			if (system == null)
			{
#if UNITY_EDITOR
				system = AssetLoadHelper.GetSystemAsset<TSystem>();
#endif
			}

			return system;
		}
	}
}