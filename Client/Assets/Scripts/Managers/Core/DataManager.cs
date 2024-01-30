using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.TestData> TestDict { get; private set; } = new Dictionary<int, Data.TestData>();

    public void Init()
    {
        TestDict = LoadJson<Data.TestDataLoader, int, Data.TestData>("TestData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("Assets/Bundle/Datas/JsonData/" + path + ".json");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}