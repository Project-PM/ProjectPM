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
        public string Name;
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
        public string Name; // Key
        
        // 어떤 스탯에 영향을 미칠 것인지
        // 얼마나 영향을 미칠 것인지
        // 어떤 캐릭터가 장착할 수 있는 장비인지
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