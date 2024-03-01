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
        DBReferenceDict[FirebaseDataType.UserInfo].Reference.ValueChanged -= OnUserInfoPropertiesUpdate;
        DBReferenceDict[FirebaseDataType.UserInfo].Reference.ValueChanged += OnUserInfoPropertiesUpdate;

        DBReferenceDict[FirebaseDataType.UserItem].Reference.ValueChanged -= OnUserItemPropertiesUpdate;
        DBReferenceDict[FirebaseDataType.UserItem].Reference.ValueChanged += OnUserItemPropertiesUpdate;
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
        userItem.item = int.Parse(e.Snapshot.Child("item").Value.ToString());

        // ��ųʸ��� ��ȯ�ؼ� ����ؾߵǴ°Ÿ� �׳� ����Ʈ�� �δ� �� ���ƺ��̱� ��
        List<object> list = e.Snapshot.Child("itemDict").Value as List<object>;

        userItem.itemDict.Clear();
        for (int i = 0; i < list.Count; i++)
            userItem.itemDict.Add(i, int.Parse(list[i].ToString()));

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