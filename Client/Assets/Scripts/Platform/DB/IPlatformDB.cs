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
        DatabaseReference dbRootReference = FirebaseDatabase.DefaultInstance.RootReference;

        for (int i = 0; i < (int)FirebaseDataType.Max; i++)
        {
            DBReferenceDict[(FirebaseDataType)i] = FirebaseDatabase.DefaultInstance.RootReference.Child(((FirebaseDataType)i).ToString()).
                Child(Managers.Platform.GetUserID());
        }

        return true;
    }

    /// <summary>
    /// ������ ��ü ���̺�
    /// </summary>
    public void SaveDB(FBUserData saveData)
    {
        DBReferenceDict[FirebaseDataType.UserInfo].SetRawJsonValueAsync(JsonConvert.SerializeObject(saveData.userInfo));
        DBReferenceDict[FirebaseDataType.UserItem].SetRawJsonValueAsync(JsonConvert.SerializeObject(saveData.userItem));
     }

    /// <summary>
    /// ������ �κ� ������Ʈ(���̺�)
    /// </summary>
    public void UpdateDB(FBDataBase updateData)
    {
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
        FBUserData userDB = new FBUserData();

        DBReferenceDict[FirebaseDataType.UserInfo].GetValueAsync().ContinueWith(task =>
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

        DBReferenceDict[FirebaseDataType.UserItem].GetValueAsync().ContinueWith(task =>
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