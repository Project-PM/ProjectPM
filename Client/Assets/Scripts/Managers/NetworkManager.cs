using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour
{
    public event Action<List<Lobby>> lobbyListChangedEvent; // 로비 목록 갱신 시 호출
    public event Action<Lobby> joinLobbyEvent; // 방 정보 갱신, 방 입장 시 호출
    public event Action leaveLobbyEvent; // 방을 나갈 때 호출
    public event Action gameStartEvent; // 게임 시작 시 호출
    private Lobby joinedLobby; // 현재 참가한 방 정보

    // 유니티 게임 서비스 로그인
    public async void Init()
    {
        // 프로필 설정
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(Managers.Platform.GetUserID()); // 프로필 설정

        await UnityServices.InitializeAsync(initializationOptions);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
