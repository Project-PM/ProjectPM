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
}
