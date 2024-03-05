using Firebase.Database;
using Firebase;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.ComponentModel;
using Unity.VisualScripting;
using System.Linq.Expressions;
using Cysharp.Threading.Tasks;
using static UnityEditor.LightingExplorerTableColumn;
using System.Reflection;
using System.IO;
using static UnityEngine.Rendering.DebugUI;

public interface IFBUserInfoPostProcess
{
    void OnUpdateFBUserInfoProperty(FBUserInfo property);
}

public interface IFBUserItemPostProcess
{
    void OnUpdateFBUserItemProperty(FBUserItem property);
}

public class FirebaseDB
{
    private Dictionary<FirebaseDataCategory, DatabaseReference> DBReferenceDict = new Dictionary<FirebaseDataCategory, DatabaseReference>();

    private List<IFBUserInfoPostProcess> userInfoProcessList = new List<IFBUserInfoPostProcess>();
    private List<IFBUserItemPostProcess> userItemProcessList = new List<IFBUserItemPostProcess>();

    /// <summary>
    /// 로그인 성공 시 최초 1번만 실행
    /// </summary>
    public bool InitDB()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            Debug.LogError("파이어베이스 앱 초기화 실패");
            return false;
        }

        DatabaseReference dbRootReference = FirebaseDatabase.DefaultInstance.RootReference;

        if (dbRootReference == null)
        {
            Debug.LogError("dbRootReference is Null!");
            return false;
        }

        string userID = Managers.Platform.GetUserID();

        for (int i = 0; i < (int)FirebaseDataCategory.Max; i++)
        {
            FirebaseDataCategory dataType = (FirebaseDataCategory)i;
            DBReferenceDict[dataType] = FirebaseDatabase.DefaultInstance
                .GetReference(dataType.ToString())
                .Child(userID);
        }

        SetUpdateCallBack();

        for (int i = 0; i < (int)FirebaseDataCategory.Max; i++)
        {
            FirebaseDataCategory category = (FirebaseDataCategory)i;
            dbRootReference.Child(category.ToString()).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    FBDataBase fbData = null;

                    switch (category)
                    {
                        case FirebaseDataCategory.UserInfo:
                            fbData = new FBUserInfo();
                            break;
                        case FirebaseDataCategory.UserItem:
                            fbData = new FBUserItem();
                            break;
                    }

                    FieldInfo[] fieldInfos = fbData.GetType().GetFields();

                    IDictionary dict = (IDictionary)snapshot.Value;

                    for (int j = 0; j < fieldInfos.Length; j++)
                    {
                        if (dict.Contains(fieldInfos[j].Name))
                        {
                            object obj = Convert.ChangeType(dict[fieldInfos[j].Name], fieldInfos[j].FieldType);
                            fieldInfos[j].SetValue(fbData, obj);
                        }
                    }

                    DBReferenceDict[category].SetRawJsonValueAsync(JsonConvert.SerializeObject(fbData));
                    Debug.Log($"{category} Data 갱신 완료");
                }
                else
                {
                    Debug.LogError("task is not Completed !");
                }
            });
        }


        return true;
    }
    
    private void SetUpdateCallBack()
    {
        DBReferenceDict[FirebaseDataCategory.UserInfo].Reference.ValueChanged -= OnUserInfoPropertiesUpdate;
        DBReferenceDict[FirebaseDataCategory.UserInfo].Reference.ValueChanged += OnUserInfoPropertiesUpdate;

        DBReferenceDict[FirebaseDataCategory.UserItem].Reference.ValueChanged -= OnUserItemPropertiesUpdate;
        DBReferenceDict[FirebaseDataCategory.UserItem].Reference.ValueChanged += OnUserItemPropertiesUpdate;
    }

    private void OnUserInfoPropertiesUpdate(object sender, ValueChangedEventArgs e)
    {
        Debug.Log($"FB UserInfo 데이터 변경 감지 {userInfoProcessList.Count}");

        FBUserInfo userInfo = new FBUserInfo();
        FieldInfo[] fieldInfos = userInfo.GetType().GetFields();
        IDictionary dict = (IDictionary)e.Snapshot.Value;

        for (int j = 0; j < fieldInfos.Length; j++)
        {
            if (dict.Contains(fieldInfos[j].Name))
            {
                object obj = Convert.ChangeType(dict[fieldInfos[j].Name], fieldInfos[j].FieldType);
                fieldInfos[j].SetValue(userInfo, obj);
            }
        }

        foreach (var process in userInfoProcessList)
        {
            process?.OnUpdateFBUserInfoProperty(userInfo);
        }
    }

    private void OnUserItemPropertiesUpdate(object sender, ValueChangedEventArgs e)
    {
        Debug.Log($"FB UserItem 데이터 변경 감지 {userItemProcessList.Count}");

        FBUserItem userItem = new FBUserItem();

        foreach (var process in userItemProcessList)
        {
            process?.OnUpdateFBUserItemProperty(userItem);
        }
    }

    public void RegisterIFBUserInfoPostProcess(IFBUserInfoPostProcess userInfoPostProcess)
    {
        if (!userInfoProcessList.Contains(userInfoPostProcess))
        {
            userInfoProcessList.Add(userInfoPostProcess);
        }
    }

    public void UnregisterIFBUserInfoPostProcess(IFBUserInfoPostProcess userInfoPostProcess)
    {
        if (userInfoProcessList.Contains(userInfoPostProcess))
        {
            userInfoProcessList.Remove(userInfoPostProcess);
        }
    }

    public void RegisterIFBUserItemPostProcess(IFBUserItemPostProcess userItemPostProcess)
    {
        if (!userItemProcessList.Contains(userItemPostProcess))
        {
            userItemProcessList.Add(userItemPostProcess);
        }
    }

    public void UnregisterIFBUserItemPostProcess(IFBUserItemPostProcess userItemPostProcess)
    {
        if (userItemProcessList.Contains(userItemPostProcess))
        {
            userItemProcessList.Remove(userItemPostProcess);
        }
    }

    /// <summary>
    /// 비어있는 데이터를 초기 값으로 만들어 줌
    /// </summary>
    public void SaveDB()
    {

    }

    /// <summary>
    /// 데이터 부분 업데이트(세이브)
    /// </summary>
    public void UpdateDB(FBDataBase updateData)
    {
        Debug.Log("DB 업데이트");
        if (updateData is FBUserInfo)
        {
            DBReferenceDict[FirebaseDataCategory.UserInfo].SetRawJsonValueAsync(JsonConvert.SerializeObject(updateData));
        }
        else if (updateData is FBUserItem)
        {
            DBReferenceDict[FirebaseDataCategory.UserItem].SetRawJsonValueAsync(JsonConvert.SerializeObject(updateData));
        }
    }

    /// <summary>
    /// 데이터 전체를 로드한다
    /// </summary>
    public bool LoadDB(Action<FBUserData> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        Debug.Log("DB 로드");
        FBUserData dataInfo = new FBUserData();

        DBReferenceDict[FirebaseDataCategory.UserInfo].GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("dataInfo 로드 실패");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                dataInfo.userInfo = JsonConvert.DeserializeObject<FBUserInfo>(snapShot.GetRawJsonValue());
            }
        });

        DBReferenceDict[FirebaseDataCategory.UserItem].GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("dataInfo 로드 실패");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                dataInfo.userItem = JsonConvert.DeserializeObject<FBUserItem>(snapShot.GetRawJsonValue());

            }
        });

        return true;
    }

    
}