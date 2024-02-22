using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System;

#region FBUserDatas
/// <summary>
/// ���̾�̽��� ����Ǵ� ������ �׷� Ÿ��
/// </summary>
public enum FirebaseDataType
{
    UserInfo,
    UserItem,
}

/// <summary>
/// ���� ������ ���̽� ����� ����,
/// �ʿ��� ��쿡�� UserData Ŭ���� ������ �÷��� ��
/// </summary>
public class FBUserData
{
    public FBUserInfo userInfo = new FBUserInfo();
    public FBUserItem userItem = new FBUserItem();
}

public class BaseFBData { }

[Serializable]
public class FBUserInfo : BaseFBData
{
    public string userKey = string.Empty; // �������� �����Ǵ� ���� ���� Ű
    public string lastLoginTime = string.Empty; // ������ ���� �ð�
    public string userNickName = string.Empty; // ���� �г��� (�ʱ⿣ ����Ű�� ����)
    public bool isLogined = false; // ���� �α��� ���� (�ߺ� �α��� ���� ó��?)
    public UserLoginType userLoginType = UserLoginType.Guest; // ���� ����ȭ ����

}

[Serializable]
public class FBUserItem : BaseFBData
{
    public Dictionary<int, int> itemDict = new Dictionary<int, int>(); // <������ ���� ��ȣ, ����>?
}
#endregion

public class PlatformManager
{
    FBUserData fbDataInfo = new FBUserData();

    
   
}
