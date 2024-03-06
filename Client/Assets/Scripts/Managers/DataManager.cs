using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : IFBUserInfoPostProcess, IFBUserItemPostProcess
{
    public Dictionary<int, Data.TestData> TestDict { get; private set; } = new Dictionary<int, Data.TestData>();
    public FBUserData MyUserData { get; private set; } = null;

    public void Init()
    {
        MyUserData = new FBUserData();
        Managers.Platform.RegisterFBUserInfoCallback(this);
        Managers.Platform.RegisterFBUserItemCallback(this);

        TestDict = LoadJson<Data.TestDataLoader, int, Data.TestData>("TestData").MakeDict();
    }

    public void Clear()
    {
        Managers.Platform.UnregisterFBUserInfoCallback(this);
        Managers.Platform.UnregisterFBUserItemCallback(this);
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("Assets/Bundle/Datas/JsonData/" + path + ".json");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

    public void OnUpdateFBUserInfoProperty(FBUserInfo property)
    {
        MyUserData.userInfo = property;
    }

    public void OnUpdateFBUserItemProperty(FBUserItem property)
    {
        MyUserData.userItem = property;
    }
}