using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;

    private int playerId = -1;

    public void Initialize(int playerId)
    {
        this.playerId = playerId;

        playerController.SetPlayerId(playerId);
	}
}
