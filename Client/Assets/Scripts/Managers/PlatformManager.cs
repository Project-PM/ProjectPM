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
using Unity.Mathematics;
using UnityEngine.UIElements;

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

    public string MyUserID { get; private set; } = null;

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

    public void GetRakingBoardDatas(Action<List<RankingBoardData>> onRankingBoardData)
    {
        DB.GetRankingBoardDatas(onRankingBoardData);
    }

    private System.Random random = new System.Random((int)DateTime.Now.Ticks & 0x0000FFFF); //랜덤 시드값

    public void PushRankingBoardData(RankingBoardData data)
    {
        // 테스트용 로직
        int lenght = 34;
        string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  //문자 생성 풀
        char[] chRandom = new char[lenght];

        for (int i = 0; i < lenght; i++)
        {
            chRandom[i] = strPool[random.Next(strPool.Length)];
        }
        string strRet = new String(chRandom);   // char to string

        DB.PushMyRankingBoardData(data, strRet);

        // 원래는 이렇게만 사용해야 함 - 매개변수 빼고 DB에서 UserID 가져와서 세팅
        // DB.PushMyRankingBoardData(data, GetUserID());
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
            OnSignInSuccess: () =>
            {
                // 로그인 성공 ( 유저 ID 세팅 ) -> 유저 ID로 DB를 갱신
                DB.InitDB();
                _OnSignInSuccess?.Invoke();
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

    public void Logout()
    {
        if (Auth.IsLogin)
            Auth.SignOut();
    }
}