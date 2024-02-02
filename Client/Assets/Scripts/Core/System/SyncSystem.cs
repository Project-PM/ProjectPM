using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SyncSystem : MonoSystem, IPacketReceiver
{
	[SerializeField] protected SessionSystem sessionSystem = null;

	public override void OnEnter(SystemParam systemParam)
	{
		base.OnEnter(systemParam);

		sessionSystem.RegisterPacketReceiver(this);
	}

	public override void OnExit()
	{
		base.OnExit();

		sessionSystem.UnRegisterPacketReceiver(this);
	}

	public bool Send(IPacket packet)
	{
		return sessionSystem.Send(packet);
	}

	public virtual void OnReceive(IPacket packet)
	{

	}
}
