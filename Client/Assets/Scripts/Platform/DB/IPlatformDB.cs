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
    /// �α��� ���� �� ���� 1���� ����
    /// </summary>
    public bool InitDB(Action OnSuccess)
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

        // UniWaitFBDataInit(OnSuccess).Forget();

        string userID = Managers.Platform.GetUserID();

        for (int i = 0; i < (int)FirebaseDataCategory.Max; i++)
        {
            FirebaseDataCategory dataType = (FirebaseDataCategory)i;
            DBReferenceDict[dataType] = FirebaseDatabase.DefaultInstance
                .GetReference(dataType.ToString())
                .Child(userID);
        }

        for (int i = 0; i < (int)FirebaseDataCategory.Max; i++)
        {
            FirebaseDataCategory category = (FirebaseDataCategory)i;
            dbRootReference.Child(category.ToString()).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    SaveDB(category, snapshot);
                }
                else
                {
                    Debug.LogError("task is not Completed !");
                }
            });
        }
        
        return true;
    }

    private void SaveDB(FirebaseDataCategory dataType, DataSnapshot snapshot)
    {
        Debug.Log(dataType);

        FBDataBase fbData = null;
        
        switch(dataType)
        {
            case FirebaseDataCategory.UserInfo:
                fbData = new FBUserInfo();
                break;
            case FirebaseDataCategory.UserItem:
                fbData = new FBUserItem();
                break;
        }

        if (fbData is FBUserItem)
        {
            FBUserItem fBUserItem = (FBUserItem)fbData;
            Debug.Log($"{fBUserItem.testNum}");
        }

        FieldInfo[] fieldInfos = fbData.GetType().GetFields();

        for (int i = 0; i < fieldInfos.Length; i++)
        {
            if (snapshot.Child(fieldInfos[i].Name) != null)
            {
                object value = snapshot.Child(fieldInfos[i].Name).Value;

                fieldInfos[i].SetValue(fbData, value);
            }
        }

        if(fbData is FBUserItem)
        {
            FBUserItem fBUserItem = (FBUserItem)fbData;
            Debug.Log($"{fBUserItem.testNum}");
        }

        DBReferenceDict[dataType].SetRawJsonValueAsync(JsonConvert.SerializeObject(fbData));
    }
    
    private async UniTask UniWaitFBDataInit(Action<bool> OnSuccess)
    {
        // �� ������ ī�װ����� 
        await UniTask.WaitUntil(() => DBReferenceDict.Count == (int)FirebaseDataCategory.Max);

        

        

        SetUpdateCallBack();

        OnSuccess?.Invoke(true);
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
        Debug.Log($"FB UserInfo ������ ���� ���� {userInfoProcessList.Count}");

        FBUserInfo userInfo = new FBUserInfo();
        userInfo.userKey = e.Snapshot.Child("userKey").Value.ToString();
        userInfo.userNickName = e.Snapshot.Child("userNickName").Value.ToString();

        foreach (var process in userInfoProcessList)
        {
            process?.OnUpdateFBUserInfoProperty(userInfo);
        }
    }

    private void OnUserItemPropertiesUpdate(object sender, ValueChangedEventArgs e)
    {
        Debug.Log($"FB UserItem ������ ���� ���� {userItemProcessList.Count}");

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
    /// ����ִ� �����͸� �ʱ� ������ ����� ��
    /// </summary>
    public void SaveDB()
    {

    }

    /// <summary>
    /// ������ �κ� ������Ʈ(���̺�)
    /// </summary>
    public void UpdateDB(FBDataBase updateData)
    {
        Debug.Log("DB ������Ʈ");
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
    /// ������ ��ü�� �ε��Ѵ�
    /// </summary>
    public bool LoadDB(Action<FBUserData> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        Debug.Log("DB �ε�");
        FBUserData dataInfo = new FBUserData();

        DBReferenceDict[FirebaseDataCategory.UserInfo].GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("dataInfo �ε� ����");
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
                Debug.Log("dataInfo �ε� ����");
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