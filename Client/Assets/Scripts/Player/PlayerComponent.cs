using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENUM_CHARACTER_TYPE
{
    Red,
    Green, 
    Blue
}

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private PlayerCharacterController playerController = null;

    public int playerId { get; private set; } = -1;

    public void SetPlayerId(int playerId)
    {
        this.playerId = playerId;
	}

    public void SetInput(bool isEnable)
    {
        playerController.SetInput(isEnable);
	}
}
