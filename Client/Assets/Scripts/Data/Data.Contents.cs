using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    #region FBUserDatas
    /// <summary>
    /// 유저 데이터 베이스 저장용 구조,
    /// 필요한 경우에는 UserData 클래스 종류를 늘려줄 것
    /// </summary>
    [Serializable]
    public class FBUserData
    {
        public FBUserInfo userInfo = new FBUserInfo();
        public FBUserItem userItem = new FBUserItem();
    }

    [Serializable]
    public class FBDataBase { }

    [Serializable]
    public class FBUserInfo : FBDataBase
    {
        public string userNickName = "Guest";
        public int userLoginType = (int)UserLoginType.Guest;
        public int useCharacterType = (int)CharacterType.Red;
    }

    [Serializable]
    public class FBUserItem : FBDataBase
    {
        public int coin = 0;
        public int characterPiece = 0; // 캐릭터 조각
        public List<string> characterGearList = new List<string>(); // 보유 캐릭터장비 목록
        public List<bool> testBoolList = new List<bool>();
        public List<int> testIntList = new List<int>();
        public int testNum = 15;
        // 캐릭터별 장착상태를 저장해야 함 
    }
    #endregion

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