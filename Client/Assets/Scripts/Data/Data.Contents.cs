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
}