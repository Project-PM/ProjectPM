using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleServer
{
    internal class BattleRoom : Room
	{
		private Dictionary<int, int> playerCharacterDictionary = new Dictionary<int, int>();

        internal BattleRoom(int roomId, int maxSessionCount) : base(roomId, maxSessionCount)
		{
		}

        protected void OnEnter(Session session, REQ_ENTER_GAME packet)
        {
			if (roomSessions.Contains(session) == false)
				roomSessions.Add(session);

			playerCharacterDictionary[session.sessionId] = packet.characterType;

			var enter = new RES_BROADCAST_ENTER_GAME();
            enter.playerId = session.sessionId;

            Broadcast(enter.Write());
		}

		protected override void OnLeave(Session session)
		{
			if (roomSessions.Contains(session))
				roomSessions.Remove(session);

			if (playerCharacterDictionary.ContainsKey(session.sessionId))
				playerCharacterDictionary.Remove(session.sessionId);

			var leave = new RES_BROADCAST_LEAVE_GAME();
            leave.playerId = session.sessionId;

            Broadcast(leave.Write());
		}

		protected override void OnReady(Session session)
		{
			var connected = new RES_CONNECTED();
			session.Send(connected.Write());
		}

		public void ResponsePlayerList(Session session)
        {
            jobQueue.Push(() =>
            {
                OnResponsePlayerList(session);
			});
        }

        private void OnResponsePlayerList(Session session)
        {
			var players = new RES_PLAYER_LIST();

			foreach (BattleSession s in roomSessions)
			{
				if (playerCharacterDictionary.TryGetValue(s.sessionId, out var characterType) == false)
					continue;

				players.players.Add(new RES_PLAYER_LIST.Player()
				{
					isSelf = s == session,
					playerId = s.sessionId,
					characterType = characterType
				});
			}

			session.Send(players.Write());
		}

		public void Enter(BattleSession session, REQ_ENTER_GAME packet)
		{
			jobQueue.Push(() =>
			{
				OnEnter(session, packet);
			});
		}
	}
}