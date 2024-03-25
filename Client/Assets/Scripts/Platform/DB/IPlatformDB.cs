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
using UnityEditor.Timeline.Actions;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using System.Runtime.Serialization.Formatters;
using System.Data.Common;

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
    private DatabaseReference DBReferenceRanking = null;

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
            FirebaseDataCategory category = (FirebaseDataCategory)i;
            DBReferenceDict[category] = FirebaseDatabase.DefaultInstance
                .GetReference(category.ToString())
                .Child(userID);
        }

        DBReferenceRanking = FirebaseDatabase.DefaultInstance.GetReference("RankingBoard");

        for (int i = 0; i < (int)FirebaseDataCategory.Max; i++)
        {
            FirebaseDataCategory category = (FirebaseDataCategory)i;

            dbRootReference.Child(category.ToString()).Child(userID).GetValueAsync().ContinueWithOnMainThread(task =>
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

                    FBDataUpdateCheck(fbData, snapshot);

                    DBReferenceDict[category].SetRawJsonValueAsync(JsonConvert.SerializeObject(fbData));
                    Debug.Log($"{category} Data 갱신 완료");
                }
                else
                {
                    Debug.LogError("task is not Completed !");
                }
            });
        }

        SetUpdateCallBack();

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
        
        FBDataUpdateCheck(userInfo, e.Snapshot);

        foreach (var process in userInfoProcessList)
        {
            process?.OnUpdateFBUserInfoProperty(userInfo);
        }
    }

    private void OnUserItemPropertiesUpdate(object sender, ValueChangedEventArgs e)
    {
        Debug.Log($"FB UserItem 데이터 변경 감지 {userItemProcessList.Count}");

        FBUserItem userItem = new FBUserItem();
        
        FBDataUpdateCheck(userItem, e.Snapshot);

        foreach (var process in userItemProcessList)
        {
            process?.OnUpdateFBUserItemProperty(userItem);
        }
    }

    private void FBDataUpdateCheck(FBDataBase fbData, DataSnapshot snapshot)
    {
        IDictionary dict = (IDictionary)snapshot.Value;

        if (dict == null)
            return;

        FieldInfo[] fieldInfos = fbData.GetType().GetFields();

        for (int j = 0; j < fieldInfos.Length; j++)
        {
            if (dict.Contains(fieldInfos[j].Name) == false)
                continue;

            if (fieldInfos[j].FieldType.IsGenericType && fieldInfos[j].FieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                List<object> objList = (List<object>)dict[fieldInfos[j].Name];

                if (fieldInfos[j].FieldType == typeof(List<string>))
                {
                    List<string> list = objList.ConvertAll(x => x.ToString());
                    fieldInfos[j].SetValue(fbData, list);
                }
                else if (fieldInfos[j].FieldType == typeof(List<int>))
                {
                    List<int> list = objList.ConvertAll(x => int.Parse(x.ToString()));
                    fieldInfos[j].SetValue(fbData, list);
                }
                else if (fieldInfos[j].FieldType == typeof(List<bool>))
                {
                    List<bool> list = objList.ConvertAll(x => (bool)x);
                    fieldInfos[j].SetValue(fbData, list);
                }
                else
                    Debug.LogWarning($"제네릭 타입 '{fieldInfos[j].FieldType}' 를 추가해주세요.");
            }
            else if (fieldInfos[j].FieldType.IsGenericType)
            {
                Debug.LogWarning($"제네릭 타입 '{fieldInfos[j].FieldType}' 를 추가해주세요.");
            }
            else if (fieldInfos[j].FieldType.IsArray)
            {
                List<object> objList = (List<object>)dict[fieldInfos[j].Name];

                if (fieldInfos[j].FieldType == typeof(string[]))
                {
                    string[] array = objList.Select(x => x.ToString()).ToArray();
                    fieldInfos[j].SetValue(fbData, array);
                }
                else if (fieldInfos[j].FieldType == typeof(int[]))
                {
                    int[] array = objList.Select(x => int.Parse(x.ToString())).ToArray();
                    fieldInfos[j].SetValue(fbData, array);
                }
                else if (fieldInfos[j].FieldType == typeof(bool[]))
                {
                    bool[] array = objList.Select(x => (bool)x).ToArray();
                    fieldInfos[j].SetValue(fbData, array);
                }
                else
                    Debug.LogWarning($"제네릭 타입 '{fieldInfos[j].FieldType}' 를 추가해주세요.");
            }
            else
            {
                object value = Convert.ChangeType(dict[fieldInfos[j].Name], fieldInfos[j].FieldType);
                fieldInfos[j].SetValue(fbData, value);
            }
        }
    }

    public void GetRankingBoardDatas(Action<List<RankingBoardData>> onRankingBoardData)
    {
        DBReferenceRanking.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                List<RankingBoardData> list = new();

                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    RankingBoardData info = new RankingBoardData();

                    info.userNickname = childSnapshot.Child("userNickname").Value.ToString();
                    info.ratingPoint = int.Parse(childSnapshot.Child("ratingPoint").Value.ToString());

                    list.Add(info);
                }

                onRankingBoardData(list);
            }
            else
            {
                Debug.LogWarning("데이터 로드 실패");
            }
        });
    }

    public void PushMyRankingBoardData(RankingBoardData data, string myUserID)
    {
        Debug.Log("랭킹보드 데이터 추가 완료");
        DBReferenceRanking.Child(myUserID).SetRawJsonValueAsync(JsonConvert.SerializeObject(data));
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