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
using UnityEngine.Rendering;

#region FBUserDatas
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
    public string userNickName = "Guest"; // 유저 닉네임 (설정 전엔 유저키로 세팅)
    public int userLoginType = (int)UserLoginType.Guest; // 계정 동기화 정보
    public int useCharacterType = (int)CharacterType.Red;
    
}
                                           
[Serializable]
public class FBUserItem : FBDataBase
{
    public int coin = 0;
    public int characterPiece = 0;
    // public List<FBChracterGear> equipmentList = new List<FBChracterGear>();
}

[Serializable]
public class FBChracterGear
{

}

[Serializable]

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

    public bool UpdateDB(FirebaseDataCategory type, FBDataBase data, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
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
        Auth.SignIn(
            OnSignInSuccess: () => // 로그인 성공 시 
            {
                DB.InitDB(_OnSignInSuccess);

                _OnSignInSuccess?.Invoke();

                /*
                _OnCheckFirstUser += (bool b) => // 체크 및 Initialize 먼저 하고, 
                {
                    _OnSignInSuccess?.Invoke(); // 성공 콜백을 호출해야 서순이 맞음
                };

                CheckFirstUserOrInitialize(_OnCheckFirstUser);
                */
            },
            OnSignInFailed: () =>
            {
                _OnSignInFailed?.Invoke();
            },
            OnSignCanceled: () =>
            {
                _OnSignCanceled?.Invoke();
            }
        );
    }

    /*
    private void CheckFirstUserOrInitialize(Action<bool> checkRoutine)
    {
        InitializeCurrentUserDB(OnCompleted: (data) =>
        {
            checkRoutine?.Invoke(data == null || data.nickname.IsNullOrEmpty());

            if (data != null)
            {
                MyCashPoint = data.purchaseCoffeeCount;
            }
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

        DB.SelectDB(hierachyPath, true, (data) =>
        {
            if (data == null)
            {
                DB.InsertDB(hierachyPath, userId);
            }

            OnCompleted?.Invoke(data);
        });
    }
    */

    public void Logout()
    {
        if (Auth.IsLogin)
            Auth.SignOut();
    }
}