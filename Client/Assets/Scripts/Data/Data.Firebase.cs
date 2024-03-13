using System;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.HostingServices;

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
    // ĳ���ͺ� �������¸� �����ؾ� �� 
}

[Serializable]
public class RankingBoardData
{
    public string userNickname = "Guest";
    public int ratingPoint = 0;
}