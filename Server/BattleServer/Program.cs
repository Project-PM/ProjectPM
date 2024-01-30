using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace BattleServer
{
    internal class Program
	{
        private static Executer executer = new Executer(MakeSession, 7777);

		private static Session MakeSession(EndPoint clientEndPoint)
		{
			return BattleSessionFactory.Make(clientEndPoint.GetHashCode(), MakeRoom);
		}

		private static BattleRoom MakeRoom(int roomId, int maxRoomMemberCount)
		{
			return BattleRoomFactory.Make(roomId, maxRoomMemberCount);
		}

        private static void Main(string[] args)
        {
            executer.Execute();
        }
    }
}
