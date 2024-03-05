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
    public string userNickName = "Guest"; // ���� �г��� (���� ���� ����Ű�� ����)
    public int userLoginType = (int)UserLoginType.Guest; // ���� ����ȭ ����
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
            Debug.LogError("���� �α����� ���� ���� �����Դϴ�.");
            return string.Empty;
        }

        return Auth.UserId;
    }

    public void Login(Action _OnSignInSuccess = null, Action _OnSignInFailed = null, Action _OnSignCanceled = null, Action<bool> _OnCheckFirstUser = null)
    {
        Auth.SignIn(
            OnSignInSuccess: () => // �α��� ���� �� 
            {
                DB.InitDB(_OnSignInSuccess);

                _OnSignInSuccess?.Invoke();

                /*
                _OnCheckFirstUser += (bool b) => // üũ �� Initialize ���� �ϰ�, 
                {
                    _OnSignInSuccess?.Invoke(); // ���� �ݹ��� ȣ���ؾ� ������ ����
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
            Debug.LogError("�α��� ���°� �ƴմϴ�.");
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