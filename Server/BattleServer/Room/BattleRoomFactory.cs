using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleServer
{
    internal class BattleRoomFactory
    {
        public static BattleRoom Make(int roomId, int maxRoomMemberCount)
        {
            return (BattleRoom)RoomManager.Instance.Make(roomId, maxRoomMemberCount, MakeRoom);
        }

        private static Room MakeRoom(int roomId, int maxRoomMemberCount)
        {
            return new BattleRoom(roomId, maxRoomMemberCount);
        }
    }
}
