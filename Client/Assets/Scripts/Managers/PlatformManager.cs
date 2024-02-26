using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using JetBrains.Annotations;
using Firebase.Extensions;

#region FBUserDatas
/// <summary>
/// 파이어베이스에 저장되는 데이터 그룹 타입
/// </summary>
public enum FirebaseDataType
{
    UserInfo,
    UserItem,
    Max,
}

/// <summary>
/// 유저 데이터 베이스 저장용 구조,
/// 필요한 경우에는 UserData 클래스 종류를 늘려줄 것
/// </summary>
[Serializable]
public class FBUserData
{
    public FBUserInfo userInfo = new FBUserInfo();
    public FBUserItem userItem = new FBUserItem();
}

[Serializable]
public class FBDataBase { }

[Serializable]
public class FBUserInfo : FBDataBase
{
    public string userKey = string.Empty; // 유저에게 공개되는 유저 고유 키
    public string lastLoginTime = string.Empty; // 마지막 접속 시간
    public string userNickName = string.Empty; // 유저 닉네임 (설정 전엔 유저키로 세팅)
    public bool isLogined = false; // 현재 로그인 상태 (중복 로그인 관련 처리?)
    public UserLoginType userLoginType = UserLoginType.Guest; // 계정 동기화 정보
}

[Serializable]
public class FBUserItem : FBDataBase
{
    public Dictionary<int, int> itemDict = new Dictionary<int, int>(); // <아이템 고유 번호, 개수>?
}
#endregion

public class PlatformManager
{
    private IPlatformAuth Auth
    {
        get
        {
            auth ??= new PlatformGuestAuth(); // 일단 무조건 게스트로그인
            return auth;
        }
    }
    private IPlatformAuth auth = null;

    IPlatformDB DB = new FirebaseDB();

    public FBUserData fbDataInfo { get; private set; } = new FBUserData();

    public void Initialize()
    {
        Clear();

        FirebaseApp.CheckAndFixDependenciesAsync()
               .ContinueWithOnMainThread(task =>
               {
                   if (task.Result == DependencyStatus.Available)
                   {
                       Auth.TryConnectAuth();
                       DB.InitDB();
                       DB.LoadDB();

                       Debug.Log("파이어베이스 인증 성공");
                   }
                   else // 호출 시도 아직 안 해봄
                   {
                       Debug.LogError("파이어베이스 인증 실패");
                   }
               });
    }

    public void Clear()
    {
        auth = new PlatformGuestAuth();
    }

    public string GetUserID()
    {
        if (!Auth.IsAuthValid)
        {
            Debug.LogError("아직 로그인이 되지 않은 상태입니다.");
            return string.Empty;
        }

        return Auth.UserId;
    }

    public void Login(Action _OnSignInSuccess = null, Action _OnSignInFailed = null, Action _OnSignCanceled = null, Action<bool> _OnCheckFirstUser = null)
    {
        Auth.SignIn(OnSignInSuccess: () => // 로그인 성공 시 
        {
            _OnCheckFirstUser += (bool b) => // 체크 및 Initialize 먼저 하고, 
            {
                _OnSignInSuccess?.Invoke(); // 성공 콜백을 호출해야 서순이 맞음
            };

            CheckFirstUserOrInitialize(_OnCheckFirstUser);
        },
        _OnSignInFailed, _OnSignCanceled);
    }

    public void Logout()
    {
        if (Auth.IsLogin)
            Auth.SignOut();
    }

    private void CheckFirstUserOrInitialize(Action<bool> checkRoutine)
    {
        InitializeCurrentUserDB(OnCompleted: (data) =>
        {
            checkRoutine?.Invoke(data == null || string.IsNullOrEmpty(data.userNickName));
        });
    }

    private void InitializeCurrentUserDB(Action<FBUserInfo> OnCompleted = null)
    {
        var loginType = Auth.CurrentLoginType;
        var userId = GetUserID();

        if (Auth.IsLogin == false)
        {
            Debug.LogError("로그인 상태가 아닙니다.");
            return;
        }


    }
}