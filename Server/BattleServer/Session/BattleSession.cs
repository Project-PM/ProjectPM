using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;

namespace BattleServer
{
    internal class BattleSession : PacketSession
	{
		private BattleRoom sessionRoom;
		private Func<int, int, BattleRoom> roomFactory;

        internal BattleSession(int sessionId, Func<int, int, BattleRoom> roomFactory) : base(sessionId)
		{
			this.roomFactory = roomFactory;
		}

		public bool OnRequestEnterGame(REQ_ENTER_GAME enterGame)
		{
			if (enterGame == null)
				return false;

			sessionRoom = roomFactory?.Invoke(enterGame.roomId, enterGame.roomMemberCount);
			if (sessionRoom == null)
				return false;

			sessionRoom.Enter(this, enterGame);
			return true;
		}

		public bool OnRequestLeaveGame()
		{
			if (sessionRoom == null)
				return false;

			sessionRoom.Leave(this);
			return true;
		}

		public bool OnRequestPlayerList()
		{
			if (sessionRoom == null)
				return false;

			sessionRoom.ResponsePlayerList(this);
			return true;
		}

		public bool OnRequestFrameInput(REQ_FRAME_INPUT frameInput)
		{
			if (sessionRoom == null)
				return false;

			sessionRoom.ResponseFrameInput(this, frameInput);
			return true;
		}

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");
			Send(new RES_CONNECTED().Write());
		}

        public override void OnReceivePacket(ArraySegment<byte> buffer)
		{
			BattlePacketManager.Instance.OnReceivePacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			SessionManager.Instance.UnRegisterSession(this);

			sessionRoom?.Leave(this);
			sessionRoom = null;

			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			Console.WriteLine($"OnSend : {numOfBytes}");
		}
	}
}
