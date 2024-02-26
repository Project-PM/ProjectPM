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
/// ���̾�̽��� ����Ǵ� ������ �׷� Ÿ��
/// </summary>
public enum FirebaseDataType
{
    UserInfo,
    UserItem,
    Max,
}

/// <summary>
/// ���� ������ ���̽� ����� ����,
/// �ʿ��� ��쿡�� UserData Ŭ���� ������ �÷��� ��
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
    public string userKey = string.Empty; // �������� �����Ǵ� ���� ���� Ű
    public string lastLoginTime = string.Empty; // ������ ���� �ð�
    public string userNickName = string.Empty; // ���� �г��� (���� ���� ����Ű�� ����)
    public bool isLogined = false; // ���� �α��� ���� (�ߺ� �α��� ���� ó��?)
    public UserLoginType userLoginType = UserLoginType.Guest; // ���� ����ȭ ����
}

[Serializable]
public class FBUserItem : FBDataBase
{
    public Dictionary<int, int> itemDict = new Dictionary<int, int>(); // <������ ���� ��ȣ, ����>?
}
#endregion

public class PlatformManager
{
    private IPlatformAuth Auth
    {
        get
        {
            auth ??= new PlatformGuestAuth(); // �ϴ� ������ �Խ�Ʈ�α���
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

                       Debug.Log("���̾�̽� ���� ����");
                   }
                   else // ȣ�� �õ� ���� �� �غ�
                   {
                       Debug.LogError("���̾�̽� ���� ����");
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
            Debug.LogError("���� �α����� ���� ���� �����Դϴ�.");
            return string.Empty;
        }

        return Auth.UserId;
    }

    public void Login(Action _OnSignInSuccess = null, Action _OnSignInFailed = null, Action _OnSignCanceled = null, Action<bool> _OnCheckFirstUser = null)
    {
        Auth.SignIn(OnSignInSuccess: () => // �α��� ���� �� 
        {
            _OnCheckFirstUser += (bool b) => // üũ �� Initialize ���� �ϰ�, 
            {
                _OnSignInSuccess?.Invoke(); // ���� �ݹ��� ȣ���ؾ� ������ ����
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
            Debug.LogError("�α��� ���°� �ƴմϴ�.");
            return;
        }


    }
}