using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoComponent<TSystem> : MonoBehaviour where TSystem : MonoSystem
{
	[SerializeField] protected TSystem system = null;

	protected virtual void Reset()
	{
		system = AssetLoadHelper.GetSystemAsset<TSystem>();
	}
}