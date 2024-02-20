using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseData : MonoBehaviour
{
    private Dictionary<FirebaseDataType, DatabaseReference> firebaseDBDict = new Dictionary<FirebaseDataType, DatabaseReference>();
    private readonly string URL = "https://projectpm-da143-default-rtdb.firebaseio.com/";

    public void Init()
    {

    }

    public void SaveFBData(FBUserData userData)
    {
        firebaseDBDict[FirebaseDataType.UserInfo].SetRawJsonValueAsync(JsonConvert.SerializeObject(userData.userInfo));
        firebaseDBDict[FirebaseDataType.UserItem].SetRawJsonValueAsync(JsonConvert.SerializeObject(userData.userItem));
    }

    public FBUserData LoadFBData()
    {
        FBUserData userDB = new FBUserData();

        firebaseDBDict[FirebaseDataType.UserInfo].GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                userDB.userInfo = JsonConvert.DeserializeObject<FBUserInfo>(snapShot.GetRawJsonValue());
            }
        });

        firebaseDBDict[FirebaseDataType.UserItem].GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                userDB.userItem = JsonConvert.DeserializeObject<FBUserItem>(snapShot.GetRawJsonValue());
            }
        });

        return userDB;
    }
}

/// <summary>
/// ���� ������ ���̽� ����� ����,
/// �ʿ��� ��쿡�� UserData Ŭ���� ������ �÷��� ��
/// </summary>
public class FBUserData
{
    public FBUserInfo userInfo = new FBUserInfo();
    public FBUserItem userItem = new FBUserItem();
}

public class BaseFBData { }

[Serializable]
public class FBUserInfo : BaseFBData
{
    public string userKey = string.Empty; // �������� �����Ǵ� ���� ���� Ű
    public string lastLoginTime = string.Empty; // ������ ���� �ð�
    public string userNickName = string.Empty; // ���� �г��� (�ʱ⿣ ����Ű�� ����)
    public bool isLogined = false; // ���� �α��� ���� (�ߺ� �α��� ���� ó��?)
    public UserLoginType userLoginType = UserLoginType.Guest; // ���� ����ȭ ����

}

[Serializable]
public class FBUserItem : BaseFBData
{
    public Dictionary<int, int> itemDict = new Dictionary<int, int>(); // <������ ���� ��ȣ, ����>?
}
