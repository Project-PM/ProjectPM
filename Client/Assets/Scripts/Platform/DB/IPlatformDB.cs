using Firebase.Database;
using Firebase;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformDB
{
    public bool InitDB();
    public void SaveDB(FBUserData saveData);
    public void UpdateDB(FBDataBase updateData);
    public FBUserData LoadDB(); // �ε�� ������ �ѹ���
}

public class FirebaseDB : IPlatformDB
{
    private Dictionary<FirebaseDataType, DatabaseReference> DBReferenceDict = new Dictionary<FirebaseDataType, DatabaseReference>();

    public bool InitDB()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            Debug.LogError("���̾�̽� �� �ʱ�ȭ ����");
            return false;
        }

        DatabaseReference dbRootReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (dbRootReference == null)
        {
            Debug.LogError("dbRootReference is Null!");
            return false;
        }

        string userID = Managers.Platform.GetUserID();

        for (int i = 0; i < (int)FirebaseDataType.Max; i++)
        {
            FirebaseDataType dataType = (FirebaseDataType)i;
            DBReferenceDict[dataType] = FirebaseDatabase.DefaultInstance
                .GetReference(dataType.ToString())
                .Child(userID);
        }

        return true;
    }

    /// <summary>
    /// ������ ��ü ���̺�
    /// </summary>
    public void SaveDB(FBUserData saveData)
    {
        if (DBReferenceDict.Count == 0)
        {
            Debug.LogWarning("Firebase Database is not initialized properly.");
            return;
        }

        DatabaseReference userInfoRef = DBReferenceDict[FirebaseDataType.UserInfo];
        DatabaseReference userItemRef = DBReferenceDict[FirebaseDataType.UserItem];

        if (userInfoRef != null)
        {
            string userInfoJson = JsonConvert.SerializeObject(saveData.userInfo);
            userInfoRef.SetRawJsonValueAsync(userInfoJson)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("userInfo ���� ���� : " + task.Exception);
                    }
                    else
                    {

                    }
                });
        }
        else
        {
            Debug.LogWarning("userInfoRef is Null!");
        }

        if (userItemRef != null)
        {
            string userItemJson = JsonConvert.SerializeObject(saveData.userItem);
            userItemRef.SetRawJsonValueAsync(userItemJson)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("userItem ���� ���� : " + task.Exception);
                    }
                    else
                    {

                    }
                });
        }
        else
        {
            Debug.LogWarning("userItemRef is NUll!");
        }
    }

    /// <summary>
    /// ������ �κ� ������Ʈ(���̺�)
    /// </summary>
    public void UpdateDB(FBDataBase updateData)
    {
        Debug.Log("DB ������Ʈ");
        if (updateData is FBUserInfo)
        {
            DBReferenceDict[FirebaseDataType.UserInfo].SetRawJsonValueAsync(JsonConvert.SerializeObject(updateData));
        }
        else if (updateData is FBUserItem)
        {
            DBReferenceDict[FirebaseDataType.UserItem].SetRawJsonValueAsync(JsonConvert.SerializeObject(updateData));
        }
    }

    /// <summary>
    /// ������ ��ü�� �ε��Ѵ�
    /// </summary>
    public FBUserData LoadDB()
    {
        Debug.Log("DB �ε�");
        FBUserData dataInfo = new FBUserData();

        DBReferenceDict[FirebaseDataType.UserInfo].GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("������ �ε� ����");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                dataInfo.userInfo = JsonConvert.DeserializeObject<FBUserInfo>(snapShot.GetRawJsonValue());

            }
        });

        DBReferenceDict[FirebaseDataType.UserItem].GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("������ �ε� ����");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                dataInfo.userItem = JsonConvert.DeserializeObject<FBUserItem>(snapShot.GetRawJsonValue());

            }
        });

        return dataInfo;
    }
}