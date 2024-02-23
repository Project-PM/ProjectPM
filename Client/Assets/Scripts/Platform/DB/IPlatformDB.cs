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
    public void UpdateDB(BaseFBData updateData);
    public FBUserData LoadDB(); // 로드는 무조건 한번에
}

public class FirebaseDB : IPlatformDB
{
    DatabaseReference firebaseRootDB = null;
    DatabaseReference firebaseLogRootDB = null;

    private Dictionary<FirebaseDataType, DatabaseReference> DBReferenceDict = new Dictionary<FirebaseDataType, DatabaseReference>();

    public bool InitDB()
    {
        firebaseRootDB = FirebaseDatabase.DefaultInstance.RootReference;

        for (int i = 0; i < (int)FirebaseDataType.Max; i++)
        {
            DBReferenceDict[(FirebaseDataType)i] = FirebaseDatabase.DefaultInstance.RootReference.Child(((FirebaseDataType)i).ToString()).
                Child(Managers.FBData.UserIDToken);
        }

        //로그용 DB 세팅해야 함
        AppOptions options = new AppOptions();
        options.AppId = ("");
        options.ApiKey = ("");
        options.DatabaseUrl = new System.Uri("https://projectpm-da143-default-rtdb.firebaseio.com/");

        FirebaseApp debugApp = FirebaseApp.Create(options);
        FirebaseDatabase secondaryDatabase = FirebaseDatabase.GetInstance(debugApp);
        firebaseLogRootDB = secondaryDatabase.RootReference;
        return true;
    }

    /// <summary>
    /// 데이터 전체 세이브
    /// </summary>
    public void SaveDB(FBUserData saveData)
    {
        DBReferenceDict[FirebaseDataType.UserInfo].SetRawJsonValueAsync(JsonConvert.SerializeObject(saveData.userInfo));
        DBReferenceDict[FirebaseDataType.UserItem].SetRawJsonValueAsync(JsonConvert.SerializeObject(saveData.userItem));
     }

    /// <summary>
    /// 데이터 부분 업데이트(세이브)
    /// </summary>
    public void UpdateDB(BaseFBData updateData)
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
    /// 데이터 전체를 로드한다
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