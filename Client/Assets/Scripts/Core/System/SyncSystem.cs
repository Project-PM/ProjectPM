using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SyncSystem : MonoSystem, IPacketReceiver
{
	[SerializeField] protected SessionSystem sessionSystem = null;

	public override void OnEnter()
	{
		base.OnEnter();

		sessionSystem.RegisterPacketReceiver(this);
	}

	public override void OnExit()
	{
		base.OnExit();

		sessionSystem.UnRegisterPacketReceiver(this);
	}

	public void Send(IPacket packet)
	{
		sessionSystem.Send(packet);
	}

	public virtual void OnReceive(IPacket packet)
	{

	}
}
