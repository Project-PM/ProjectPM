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
    public string userNickName = string.Empty; // 유저 닉네임 (설정 전엔 유저키로 세팅)
    public UserLoginType userLoginType = UserLoginType.Guest; // 계정 동기화 정보
}

[Serializable]
public class FBUserItem : FBDataBase
{
    public Dictionary<int, int> itemDict = new Dictionary<int, int>() { { 0, 0 } }; // <아이템 고유 번호, 개수>
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
                       
                       Debug.Log("파이어베이스 인증 성공");
                   }
                   else
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
            DB.InitDB();
            FBUserData dataInfo = DB.LoadDB();

            if (dataInfo == null || string.IsNullOrEmpty(dataInfo.userInfo.userKey))
            {
                // 최초 로그인인 경우
                Debug.Log("최초 로그인");
                fbDataInfo.userInfo.userKey = "userKey";
                fbDataInfo.userInfo.userNickName = fbDataInfo.userInfo.userKey;
                DB.SaveDB(fbDataInfo);
            }
            else
                fbDataInfo = dataInfo;
        },
        _OnSignInFailed, _OnSignCanceled);
    }

    public void Logout()
    {
        if (Auth.IsLogin)
            Auth.SignOut();
    }
}