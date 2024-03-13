using System;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.HostingServices;

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
    // 캐릭터별 장착상태를 저장해야 함 
}

[Serializable]
public class RankingBoardData
{
    public string userNickname = "Guest";
    public int ratingPoint = 0;
}