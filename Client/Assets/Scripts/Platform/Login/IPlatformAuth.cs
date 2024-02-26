using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Linq;

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

}

public class PlatformGuestAuth : IPlatformAuth
{
    public bool IsAuthValid => isAuthValid;
    private bool isAuthValid = false;

    public bool IsLogin => isLogin;
    private bool isLogin = false;

    public string UserId
    {
        get
        {
            string host = System.Net.Dns.GetHostName();
            var entry = System.Net.Dns.GetHostEntry(host);
            var ipAddr = entry.AddressList;
            var address = ipAddr.FirstOrDefault();

            return address.ToString().Replace(".", "").Replace(":", "") + host.Replace(".", "").Replace(":", "");
        }
    }

    public UserLoginType CurrentLoginType => UserLoginType.Guest;

    public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null)
    {
        isAuthValid = true;
        OnConnectAuthSuccess?.Invoke();
        return true;
    }

    public void SignIn(Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        isLogin = true;
        OnSignInSuccess?.Invoke();
    }

    public void SignOut()
    {
        isLogin = false;
    }
}
