using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour, IFBUserInfoPostProcess, IFBUserItemPostProcess
{
    private void Start()
    {
        Managers.Platform.Initialize();

        Managers.Platform.RegisterFBUserInfoCallback(this);
        Managers.Platform.RegisterFBUserItemCallback(this);
    }

    private void OnDestroy()
    {
        Managers.Platform.UnregisterFBUserInfoCallback(this);
        Managers.Platform.UnregisterFBUserItemCallback(this);
    }

    public void OnClickGuestLogin()
    {
        Managers.Platform.Login(() =>
        {
            string id = Managers.Platform.GetUserID();
            Debug.Log($"회원번호 : {id} 으로 로그인 완료");

        }, () =>
        {
            Debug.Log($"로그인 실패...");
        }, null);
    }

    public void OnClickGuestLogout()
    {
        Managers.Platform.Logout();
    }

    public void OnClickTestItemCount()
    {
        FBUserItem fBUserItem = new FBUserItem();
        fBUserItem.itemDict.Add(1, 5);
        Managers.Platform.UpdateDB(FirebaseDataType.UserItem, fBUserItem);
    }

    public void OnUpdateFBUserInfoProperty(FBUserInfo property)
    {
        Debug.Log($"UserInfo 갱신 - UserKey : {property.userKey}");
    }

    public void OnUpdateFBUserItemProperty(FBUserItem property)
    {
        foreach(var item in property.itemDict)
        {
            Debug.Log($"{item.Key}, {item.Value}");
        }
    }
}
