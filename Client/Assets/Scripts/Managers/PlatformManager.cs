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
    public string userNickName = string.Empty; // ���� �г��� (���� ���� ����Ű�� ����)
    public UserLoginType userLoginType = UserLoginType.Guest; // ���� ����ȭ ����
}

[Serializable]
public class FBUserItem : FBDataBase
{
    public Dictionary<int, int> itemDict = new Dictionary<int, int>() { { 0, 0 } }; // <������ ���� ��ȣ, ����>
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
                       
                       Debug.Log("���̾�̽� ���� ����");
                   }
                   else
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
            DB.InitDB();
            FBUserData dataInfo = DB.LoadDB();

            if (dataInfo == null || string.IsNullOrEmpty(dataInfo.userInfo.userKey))
            {
                // ���� �α����� ���
                Debug.Log("���� �α���");
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