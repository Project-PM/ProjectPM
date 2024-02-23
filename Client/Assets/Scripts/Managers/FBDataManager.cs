using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using JetBrains.Annotations;

#region FBUserDatas
/// <summary>
/// 파이어베이스에 저장되는 데이터 그룹 타입
/// </summary>
public enum FirebaseDataType
{
    UserInfo,
    UserItem,
    Max,
}

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
public class BaseFBData { }

[Serializable]
public class FBUserInfo : BaseFBData
{
    public string userKey = string.Empty; // 유저에게 공개되는 유저 고유 키
    public string lastLoginTime = string.Empty; // 마지막 접속 시간
    public string userNickName = string.Empty; // 유저 닉네임 (초기엔 유저키로 세팅)
    public bool isLogined = false; // 현재 로그인 상태 (중복 로그인 관련 처리?)
    public UserLoginType userLoginType = UserLoginType.Guest; // 계정 동기화 정보
}

[Serializable]
public class FBUserItem : BaseFBData
{
    public Dictionary<int, int> itemDict = new Dictionary<int, int>(); // <아이템 고유 번호, 개수>?
}
#endregion

public class FBDataManager
{
    FBUserData fbDataInfo = new FBUserData();
    IPlatformDB userDataBase;

    private string userIDToken = "";
    public string UserIDToken
    {
        get
        {
            return userIDToken;
        }
    }

    
}