using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using System.Text;

public enum UserLoginType
{
    None,
    Guest,
    Google,
}

public interface IPlatformAuth
{
    public bool IsAuthValid
    {
        get;
    }

    public bool IsLogin
    {
        get;
    }

    public string UserId
    {
        get;
    }

    public UserLoginType CurrentLoginType
    {
        get;
    }

    public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null);
    public void SignIn(Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null);
    public void SignOut();
    public void RegistStateChanged(EventHandler handler);
    public void UnregistStateChanged(EventHandler handler);

}
