using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using JetBrains.Annotations;
using Firebase.Extensions;
using Unity.VisualScripting;

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
    public Dictionary<int, int> itemDict = new Dictionary<int, int>(); // <아이템 고유 번호, 개수>
    public int item = 0;
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

    FirebaseDB DB = new FirebaseDB();

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

    // 테스트용
    public void RegisterFBUserInfoCallback(IFBUserInfoPostProcess mono)
    {
        DB.RegisterIFBUserInfoPostProcess(mono);
    }

    public void UnregisterFBUserInfoCallback(IFBUserInfoPostProcess mono)
    {
        DB.UnregisterIFBUserInfoPostProcess(mono);
    }

    public void RegisterFBUserItemCallback(IFBUserItemPostProcess mono)
    {
        DB.RegisterIFBUserItemPostProcess(mono);
    }

    public void UnregisterFBUserItemCallback(IFBUserItemPostProcess mono)
    {
        DB.UnregisterIFBUserItemPostProcess(mono);
    }

    public bool UpdateDB(FirebaseDataType type, FBDataBase data, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        if(data == null || GetUserID() == string.Empty)
        {
            OnCanceled?.Invoke();
            return false;
        }

        DB.UpdateDB(data);
        return true;
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

            // 최초 로그인 판단해서 무언가 해야 됨니다.
            // 다른 유저의 DB도 가져올 일이 생길 수도 있기 때문에
            // 그냥 나도 SelectDB 같은거 하나 추가해서 없으면 내부적으로 알아서 생성하게 하자

            // 즉, LoadDB -> SelectDB 변경하고 최초로그인 처리 ㄱㄱ
        },
        _OnSignInFailed, _OnSignCanceled);
    }

    public void Logout()
    {
        if (Auth.IsLogin)
            Auth.SignOut();
    }
}