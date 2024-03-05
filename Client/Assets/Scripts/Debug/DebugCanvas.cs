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
            Debug.Log($"ȸ����ȣ : {id} ���� �α��� �Ϸ�");

        }, () =>
        {
            Debug.Log($"�α��� ����...");
        }, null);
    }

    public void OnClickGuestLogout()
    {
        Managers.Platform.Logout();
    }

    public void OnClickTestItemCount()
    {
        FBUserItem fBUserItem = new FBUserItem();
        Managers.Platform.UpdateDB(FirebaseDataCategory.UserItem, fBUserItem);
    }

    public void OnUpdateFBUserInfoProperty(FBUserInfo property)
    {
        Debug.Log($"UserInfo ���� �ݹ�");        
    }

    public void OnUpdateFBUserItemProperty(FBUserItem property)
    {
        Debug.Log($"UserItem ���� �ݹ�");
    }
}
