using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine;
using Firebase;
using System;

/// <summary>
/// ���̾�̽� �׽�Ʈ ��ũ��Ʈ
/// </summary>
public class FirebaseController : MonoBehaviour
{
    private FirebaseAuth auth; // ��������
    private FirebaseUser user; // ��������

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                FirebaseInit();

                Debug.Log("���̾�̽� ���� ����");
            }
            else
            {
                Debug.LogError("���̾�̽� ���� ����");
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
    /// ���� ���� ���� �� ȣ��Ǵ� �޼���
    /// </summary>
    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if(senderAuth != null)
        {
            user = senderAuth.CurrentUser;

            if(user != null)
            {
                Debug.Log($"���� ���� ID : {user.UserId}");
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
                Debug.Log("�Խ�Ʈ �α��� ����");
            }
            else if(task.IsFaulted)
            {
                Debug.LogError("�Խ�Ʈ �α��� ����");
            }
        });
    }
}
