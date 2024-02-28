using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    private void Start()
    {
        Managers.Platform.Initialize();

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
}
