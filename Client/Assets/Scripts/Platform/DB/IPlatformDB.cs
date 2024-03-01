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
    private Dictionary<FirebaseDataType, DatabaseReference> DBReferenceDict = new Dictionary<FirebaseDataType, DatabaseReference>();

    private List<IFBUserInfoPostProcess> userInfoProcessList = new List<IFBUserInfoPostProcess>();
    private List<IFBUserItemPostProcess> userItemProcessList = new List<IFBUserItemPostProcess>();

    FBUserData userData = null;

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

        SetUpdateCallBack();

        return true;
    }

    private void SetUpdateCallBack()
    {
        DBReferenceDict[FirebaseDataType.UserInfo].Reference.ChildChanged -= OnUserInfoPropertiesUpdate;
        DBReferenceDict[FirebaseDataType.UserInfo].Reference.ChildChanged += OnUserInfoPropertiesUpdate;

        DBReferenceDict[FirebaseDataType.UserItem].Reference.ChildChanged -= OnUserItemPropertiesUpdate;
        DBReferenceDict[FirebaseDataType.UserItem].Reference.ChildChanged += OnUserItemPropertiesUpdate;
    }

    private void OnUserInfoPropertiesUpdate(object sender, ChildChangedEventArgs e)
    {
        Debug.Log("FB UserInfo ������ ���� ����");

        DBReferenceDict[FirebaseDataType.UserInfo].GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("ĵ��ĵ��");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("FB UserInfo ������ ���� ó��");
                DataSnapshot snapshot = task.Result;

                // �ȵ�
                Dictionary<string, object> dictUser = (Dictionary<string, object>)snapshot.Value; //�������� ������.
                
                // ���� �ȵ�
                FBUserInfo userInfo = dictUser.ToObject<FBUserInfo>();

                foreach (var process in userInfoProcessList)
                {
                    process?.OnUpdateFBUserInfoProperty(userInfo);
                }
            }
        });
    }

    private void OnUserItemPropertiesUpdate(object sender, ChildChangedEventArgs e)
    {
        Debug.Log("FB UserItem ������ ���� ����");

        DBReferenceDict[FirebaseDataType.UserItem].GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("ĵ��ĵ��");
            }
            else if (task.IsCompleted)
            {
                Debug.Log($"FB UserItem ������ ���� ó�� : {userItemProcessList.Count}���� ����");
                DataSnapshot snapshot = task.Result;

                FBUserItem userItem = new FBUserItem();

                // ����ȵ�
                userItem.item = (int)snapshot.Child("item").GetValue(true);

                foreach (var process in userItemProcessList)
                {
                    process?.OnUpdateFBUserItemProperty(userItem);
                }
            }
        });
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
    /// ������ �߰�
    /// </summary>
    public void InsertDB(FBUserData saveData)
    {
        if (DBReferenceDict.Count == 0)
        {
            Debug.LogWarning("InitDB ȣ�� ���� InserDB�� ȣ�� ��");
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
            Debug.LogWarning("userItemRef is Null!");
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
    public FBUserData LoadDB(Action<FBUserData> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        Debug.Log("DB �ε�");
        FBUserData dataInfo = new FBUserData();

        DBReferenceDict[FirebaseDataType.UserInfo].GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                dataInfo.userInfo = JsonConvert.DeserializeObject<FBUserInfo>(snapShot.GetRawJsonValue());
            }
        });

        DBReferenceDict[FirebaseDataType.UserItem].GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {

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