using ServerCore;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public interface IPacketReceiver
{
	void OnReceive(IPacket packet);
}

public class SessionHelper
{
	public static ProtocolType GetProtocolType(SessionType sessionType)
	{
		switch(sessionType)
		{
			case SessionType.Match:
				return ProtocolType.Tcp;

			case SessionType.Battle:
				return ProtocolType.Udp;
		}

		return ProtocolType.Tcp;
	}

    public static int GetPortNumber(SessionType type)
    {
        switch (type)
        {
            case SessionType.Battle:
                return 7777;

            case SessionType.Match:
                return 7778;
        }

        return 7777;
    }
}

public class SessionSystem : MonoSystem
{
    [SerializeField] private SessionType sessionType;
    [SerializeField] private int reconnectCount = 5;

    private PacketSession packetSession = null;

	private SessionConnector connector = null;
	private PacketQueue packetQueue = new PacketQueue();

	private List<IPacketReceiver> packetReceivers = new();
	private Queue<IPacketReceiver> pendingPacketReceiverQueue = new();
	
    public override void OnEnter()
    {
        base.OnEnter();

        connector = new SessionConnector(SessionHelper.GetProtocolType(sessionType), reconnectCount);
		InGamePacketManager.Instance._eventHandler += OnReceivePacket;
	}

	public override void OnExit()
	{
		InGamePacketManager.Instance._eventHandler -= OnReceivePacket;

		base.OnExit();
	}

	public override void OnUpdate(int deltaFrameCount, float deltaTime)
	{
		base.OnUpdate(deltaFrameCount, deltaTime);

		if (IsConnectedSession() == false)
			return;

		FlushSession();
	}

	private bool IsConnectedSession()
	{
		if (connector == null)
			return false;

		if (connector.IsConnected == false)
			return false;

		return true;
	}

	private void FlushSession()
	{
		while (pendingPacketReceiverQueue.TryDequeue(out var pendingReceiver))
		{
			if (packetReceivers.Contains(pendingReceiver))
				continue;

			packetReceivers.Add(pendingReceiver);
		}

		foreach (var packet in packetQueue.PopAll())
		{
			foreach (var receiver in packetReceivers)
			{
				receiver.OnReceive(packet);
			}
		}
	}

	public void Send(IPacket packet)
	{
		if (packetSession == null)
		{
			Debug.LogError($"{packet.GetType()} : 세션이 활성화되지 않은 상태에서 패킷 전송을 시도합니다.");
			return;
		}

		packetSession.Send(packet.Write());
	}

	public void RegisterPacketReceiver(IPacketReceiver receiver)
	{
		if (receiver == null)
			return;

		pendingPacketReceiverQueue.Enqueue(receiver);
	}

	public void UnRegisterPacketReceiver(IPacketReceiver receiver)
	{
		if (receiver == null)
			return;

		if (packetReceivers.Contains(receiver) == false)
			return;

		packetReceivers.Remove(receiver);
	}

	public bool TryConnect()
	{
		var endPoint = GetMyEndPoint(SessionHelper.GetPortNumber(sessionType));
		if (endPoint == null)
			return false;

		if (connector == null)
			return false;

		if (connector.IsConnected)
			return false;

		connector.Connect(endPoint, MakeSession);
		return true;
	}

	public void Disconnect()
	{
		connector.Disconnect();
	}

	private void OnReceivePacket(PacketSession session, IPacket packet)
	{
		packetQueue.Push(packet);
	}

	private PacketSession MakeSession()
	{
		if (packetSession == null)
		{
			var session = SessionFactory.Instance.Make(sessionType);
			if (session is PacketSession pSession)
			{
				packetSession = pSession;
				return packetSession;
			}
		}

		return packetSession;
	}

	private IPEndPoint GetMyEndPoint(int portNumber)
	{
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddress = ipHost.AddressList.FirstOrDefault();

		return new IPEndPoint(ipAddress, portNumber);
	}


}