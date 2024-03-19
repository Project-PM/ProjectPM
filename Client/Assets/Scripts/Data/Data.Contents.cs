using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    #region TestData
    [Serializable]
    public class TestData
    {
        public int DataId;
        public string TestString;
        public float TestFloat;
        public bool TestBool;
    }

    [Serializable]
    public class TestDataLoader : ILoader<int, TestData>
    {
        public List<TestData> tests = new List<TestData>();
        public Dictionary<int, TestData> MakeDict()
        {
            Dictionary<int, TestData> dict = new Dictionary<int, TestData>();
            foreach (TestData test in tests)
                dict.Add(test.DataId, test);
            return dict;
        }
    }
    #endregion

    #region CharacterGearData
    [Serializable]
    public class CharacterGearData
    {
        // 캐릭터장비하나에 대한 데이터
        // 데이터의 영문명은 어떻게 할지 서로 정해서 공유해야 함
        // -> 데이터 영문명을 좀 정해주시면 너무너무 감사할 거 같다....ㅎ

        public string Name; // Key
        // 어떤 부위의 장비인지? -> 
        // 어떤 캐릭터가 장착할 수 있는 장비인지? -> 
        // 얼마나 영향을 미칠 것인지?

        // -> 지금 기획 상으로 장비마다 하나의 능력치에만 영향을 미치는 것 맞는지 한번더 체크
    }

    [Serializable]
    public class CharacterGearDataaLoader : ILoader<string,  CharacterGearData>
    {
        public List<CharacterGearData> characterGearDatas = new List<CharacterGearData>();
        public Dictionary<string,  CharacterGearData> MakeDict()
        {
            Dictionary<string, CharacterGearData> dict = new Dictionary<string, CharacterGearData>();
            foreach (CharacterGearData data in characterGearDatas)
                dict.Add(data.Name, data);
            return dict;
        }
    }
    #endregion
}