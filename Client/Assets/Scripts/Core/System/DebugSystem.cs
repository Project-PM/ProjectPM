using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoSystem
{
    [SerializeField] private bool useGravity = true;

    [SerializeField] private Vector2 myPlayerPos = Vector2.zero;
    [SerializeField] private Vector2 otherPlayerPos = Vector2.zero;

    [SerializeField] private ENUM_CHARACTER_TYPE myCharacterType = ENUM_CHARACTER_TYPE.Red;
    [SerializeField] private ENUM_CHARACTER_TYPE otherCharacterType = ENUM_CHARACTER_TYPE.Red;

    [SerializeField] private PlayerComponent playerComponentPrefab = null;

    public void Spawn()
    {
        var myPlayerComponent = Instantiate(playerComponentPrefab, myPlayerPos, Quaternion.identity);
        myPlayerComponent.SetPlayerInfo(playerId, myCharacterType);

        var otherPlayerComponent = Instantiate(playerComponentPrefab, otherPlayerPos, Quaternion.identity);
        otherPlayerComponent.SetPlayerInfo(-1, otherCharacterType);

        var groundCheckComponents = FindObjectsOfType<PlayerGroundCheckComponent>();
		if (groundCheckComponents == null)
			return;

        foreach(var groundCheckComponent in groundCheckComponents)
        {
            groundCheckComponent.UseGravity = useGravity;
        }
    }
}
