using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
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

    string userToken = "asdf1234";

    FBUserData fbDataInfo = new FBUserData();

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        
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

    public void ReadTestData()
    {
        // DB의 시작 위치 세팅
        DatabaseReference testDB = FirebaseDatabase.DefaultInstance.GetReference("TestData");

        testDB.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogWarning("테스트 데이터 로드 실패");
                return;
            }
            if(task.IsCanceled)
            {
                Debug.LogWarning("테스트 데이터 로드 취소");
                return;
            }

            DataSnapshot snapshot = task.Result;
            Debug.Log(snapshot.ChildrenCount);

            foreach(var message in snapshot.Children)
            {
                Debug.Log($"{message.Key} : {message.Child("username").Value.ToString()} , {message.Child("value").Value.ToString()}");
            }
        });
    }
    
    public class TestDataClass
    {
        public string nickname;
        public int num;

        public TestDataClass(string nickname, int num)
        {
            this.nickname = nickname;
            this.num = num;
        }
    }


    public void OnClickGuestLogin()
    {
        GuestLogin();
    }

    public void OnClikcGuestLogout()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }

    private Task GuestLogin()
    {
        return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("게스트 로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogWarning("게스트 로그인 실패");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("게스트 로그인 성공");
        });
    }

    #region 이메일 로그인
    // 이메일 로그인은 테스트도 너무 귀찮은 거 같다 ㅎㅎ;;;
    public void JoinEmailMembership(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("이메일 회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {

                Debug.LogWarning("이메일 회원가입 실패");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("이메일 회원가입 완료");
        });
    }

    private void EmailLogin(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("이메일 로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {

                Debug.LogWarning("이메일 로그인 실패");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("이메일 로그인 완료");
        });
    }
    #endregion
}
