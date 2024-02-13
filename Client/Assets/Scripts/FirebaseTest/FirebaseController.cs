using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine;
using Firebase;
using System;

/// <summary>
/// 파이어베이스 테스트 스크립트
/// </summary>
public class FirebaseController : MonoBehaviour
{
    private FirebaseAuth auth; // 인증정보
    private FirebaseUser user; // 유저정보

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                FirebaseInit();

                Debug.Log("파이어베이스 인증 성공");
            }
            else
            {
                Debug.LogError("파이어베이스 인증 실패");
            }
        });
    }

    private void FirebaseInit()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged -= AuthStateChanged;
        auth.StateChanged += AuthStateChanged;
    }

    /// <summary>
    /// 인증 정보 변경 시 호출되는 메서드
    /// </summary>
    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if(senderAuth != null)
        {
            user = senderAuth.CurrentUser;

            if(user != null)
            {
                Debug.Log($"유저 정보 ID : {user.UserId}");
            }
        }
    }

    public void OnClickGuestLogin()
    {
        GuestLogin();
    }

    private Task GuestLogin()
    {
        return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {
                Debug.Log("게스트 로그인 성공");
            }
            else if(task.IsFaulted)
            {
                Debug.LogError("게스트 로그인 실패");
            }
        });
    }
}
