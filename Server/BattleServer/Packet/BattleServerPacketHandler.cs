using BattleServer;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

public partial class BattlePacketManager
{
	private void ON_REQ_ENTER_GAME(PacketSession session, IPacket packet)
	{
		var enterPacket = packet as REQ_ENTER_GAME;

		if (session is BattleSession battleSession)
		{
			battleSession.OnRequestEnterGame(enterPacket);
		}
	}

	private void ON_REQ_LEAVE_GAME(PacketSession session, IPacket packet)
	{
		if (session is BattleSession battleSession)
		{
			battleSession.OnRequestLeaveGame();
		}
	}

	private void ON_REQ_PLAYER_LIST(PacketSession session, IPacket packet)
	{
		if (session is BattleSession battleSession)
		{
			battleSession.OnRequestPlayerList();
		}
	}

	private void ON_REQ_FRAME_INPUT(PacketSession session, IPacket packet)
	{
		var frameInput = packet as REQ_FRAME_INPUT;

		if (session is BattleSession battleSession)
		{
			battleSession.OnRequestFrameInput(frameInput);
		}
	}
}