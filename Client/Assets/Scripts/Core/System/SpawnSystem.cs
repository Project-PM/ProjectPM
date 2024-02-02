using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnSystem : SyncSystem
{
    [SerializeField] private PlayerComponent playerComponentPrefab;
	[SerializeField] private Vector3 spawnPos = Vector3.zero;
	[SerializeField] private ENUM_CHARACTER_TYPE characterType;

	private Dictionary<int, PlayerComponent> playerDictionary = new Dictionary<int, PlayerComponent>();

	public override void OnEnter(SystemParam param)
	{
		base.OnEnter(param);

		// sessionSystem.TryConnect();
	}

	public void Spawn()
	{
		sessionSystem.TryConnect();
	}

	public override void OnReceive(IPacket packet)
	{
		if (packet is RES_CONNECTED connectedPacket)
		{
			var enterPacket = new REQ_ENTER_GAME();
			enterPacket.characterType = (int)characterType;
			Send(enterPacket);
		}
		else if (packet is RES_BROADCAST_ENTER_GAME enterPacket ||
			packet is RES_BROADCAST_LEAVE_GAME leavePacket)
		{
			REQ_PLAYER_LIST req = new REQ_PLAYER_LIST();
			Send(req);
		}
		else if (packet is RES_PLAYER_LIST playerListPacket)
		{
			var leftPlayerIds = GetLeftPlayerIds(playerListPacket.players).ToList();

			foreach (var id in leftPlayerIds)
			{
				if (playerDictionary.TryGetValue(id, out var controller))
				{
					playerDictionary.Remove(id);
					Destroy(controller.gameObject);
				}
			}

			foreach (var player in playerListPacket.players)
			{
				if (playerDictionary.ContainsKey(player.playerId))
					continue;

				var controller = CreatePlayer(player.playerId, player.isSelf, spawnPos);
				playerDictionary[player.playerId] = controller;
			}
		}
	}

	private IEnumerable<int> GetLeftPlayerIds(IEnumerable<RES_PLAYER_LIST.Player> players)
	{
		foreach (var playerInfo in playerDictionary)
		{
			if (playerInfo.Value == null)
				yield break;

			bool isContain = players.Any(p => p.playerId == playerInfo.Key);
			if (isContain)
				continue;

			yield return playerInfo.Key;
		}
	}

	private PlayerComponent CreatePlayer(int playerId, bool isSelf, Vector3 initialPos)
	{
		var playerComponent = Instantiate(playerComponentPrefab, initialPos, Quaternion.identity);
		if (playerComponent == null)
			return null;

		playerComponent.SetPlayerInfo(playerId, ENUM_CHARACTER_TYPE.Red);
		return playerComponent;
	}
}
