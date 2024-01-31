using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENUM_CHARACTER_TYPE
{
    None = -1,
    Red,
    Green, 
    Blue
}

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private PlayerCharacterController playerController = null;

    private int playerId = -1;
	private ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.None;


	public void SetPlayerInfo(int playerId, ENUM_CHARACTER_TYPE characterType)
    {
        this.playerId = playerId;
        this.characterType = characterType;

        foreach (var actionComponent in GetComponentsInChildren<PlayerFrameActionComponent>())
        {
            actionComponent.SetPlayerCharacterType(characterType);
        }
	}

    public void SetInput(bool isEnable)
    {
        playerController.SetInput(isEnable);
	}
}
