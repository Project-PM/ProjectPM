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
    public Dictionary<int, int> itemDict = new Dictionary<int, int>(); // <������ ���� ��ȣ, ����>
    public int item = 0;
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

    // �׽�Ʈ��
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

            // ���� �α��� �Ǵ��ؼ� ���� �ؾ� �ʴϴ�.
            // �ٸ� ������ DB�� ������ ���� ���� ���� �ֱ� ������
            // �׳� ���� SelectDB ������ �ϳ� �߰��ؼ� ������ ���������� �˾Ƽ� �����ϰ� ����

            // ��, LoadDB -> SelectDB �����ϰ� ���ʷα��� ó�� ����
        },
        _OnSignInFailed, _OnSignCanceled);
    }

    public void Logout()
    {
        if (Auth.IsLogin)
            Auth.SignOut();
    }
}