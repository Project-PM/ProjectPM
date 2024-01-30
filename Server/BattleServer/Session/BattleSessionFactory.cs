using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;

namespace BattleServer
{
	internal class BattleSessionFactory
	{
        internal static Session Make(int clientHashCode, Func<int, int, BattleRoom> roomFactory)
		{
			var session = new BattleSession(clientHashCode, roomFactory);
			if (session == null)
				return null;

			SessionManager.Instance.RegisterSession(session);
			return session;
		}
	}
}
