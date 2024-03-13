using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    #region FBUserDatas
    /// <summary>
    /// ���� ������ ���̽� ����� ����,
    /// �ʿ��� ��쿡�� UserData Ŭ���� ������ �÷��� ��
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
        public int characterPiece = 0; // ĳ���� ����
        public List<string> characterGearList = new List<string>(); // ���� ĳ������� ���
        public List<bool> testBoolList = new List<bool>();
        public List<int> testIntList = new List<int>();
        public int testNum = 15;
        // ĳ���ͺ� �������¸� �����ؾ� �� 
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
        // ĳ��������ϳ��� ���� ������
        // �������� �������� ��� ���� ���� ���ؼ� �����ؾ� ��
        // -> ������ �������� �� �����ֽø� �ʹ��ʹ� ������ �� ����....��

        public string Name; // Key
        // � ������ �������? -> 
        // � ĳ���Ͱ� ������ �� �ִ� �������? -> 
        // �󸶳� ������ ��ĥ ������?

        // -> ���� ��ȹ ������ ��񸶴� �ϳ��� �ɷ�ġ���� ������ ��ġ�� �� �´��� �ѹ��� üũ
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