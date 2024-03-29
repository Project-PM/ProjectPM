using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.HostingServices;

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
}
[Serializable]
public class FBUserItem : FBDataBase
{
    public int coin = 0;
    public int characterPiece = 0; // 캐릭터 조각
    public List<string> characterGearList = new List<string>(); // 보유 캐릭터장비 목록
    public List<bool> testBoolList = new List<bool>();
    public List<int> testIntList = new List<int>();
}
public enum FirebaseDataCategory
{
    UserInfo = 0,
    UserItem = 1,
    Max = 2,
}

[Serializable]
public class FBCharGear : FBDataBase
{
}

public class CharacterUserGear
{
    public int test1 = 1;
    public int test2 = 2;
    public int test3 = 3;
    public int test4 = 4;

}

[Serializable]
public class RankingBoardData
{
    public string userNickname = "Guest";
    public int ratingPoint = 0;
}