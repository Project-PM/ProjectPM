using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour, IFBUserInfoPostProcess, IFBUserItemPostProcess
{
    private void Start()
    {
        // 어드레서블 패치, 제이슨 데이터 로드
        StartLoadAssets();

        // 플랫폼 초기화
        Managers.Platform.Initialize();

        // 데이터 갱신 감지
        Managers.Platform.RegisterFBUserInfoCallback(this);
        Managers.Platform.RegisterFBUserItemCallback(this);
    }

    private void OnDestroy()
    {
        Managers.Platform.UnregisterFBUserInfoCallback(this);
        Managers.Platform.UnregisterFBUserItemCallback(this);
    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.Data.Init();
            }
        });
    }

    public void OnClickGuestLogin()
    {
        Managers.Platform.Login(() =>
        {
            string id = Managers.Platform.GetUserID();
            Debug.Log($"회원번호 : {id} 으로 로그인 완료");

        }, () =>
        {
            Debug.Log($"로그인 실패...");
        }, null);
    }

    public void OnClickGuestLogout()
    {
        Managers.Platform.Logout();
    }

    public void OnClickAddDBData()
    {
        FBUserItem fbUserItem = Managers.Data.MyUserData.userItem;
        fbUserItem.characterGearList.Add($"{Random.Range(0, 1000)}번 아이템명");
        fbUserItem.testIntList.Add(Random.Range(0, 1000));
        fbUserItem.testBoolList.Add((Random.value > 0.5f));
        Managers.Platform.UpdateDB(FirebaseDataCategory.UserItem, fbUserItem);
    }

    public void OnClickInitDBData()
    {
        FBUserItem fbUserItem = new();
        Managers.Platform.UpdateDB(FirebaseDataCategory.UserItem, fbUserItem);
    }

    public void OnClickRandomPicker()
    {
        List<RandomPickerElement> list = new List<RandomPickerElement>();
        
        list.Add(new RandomPickerElement("0", 10));
        list.Add(new RandomPickerElement("1", 52));
        list.Add(new RandomPickerElement("2", 10));
        list.Add(new RandomPickerElement("3", 113));
        list.Add(new RandomPickerElement("4", 10));

        int[] intArray = new int[list.Count];

        for (int i = 0; i < 100000; i++)
        {
            string str = WeightedRandomPicker.GetRandomPicker(list);
            int num = int.Parse(str);
            intArray[num]++;
        }

        for(int i = 0; i < intArray.Length; i++)
        {
            Debug.Log($"{i}번 : {intArray[i]}");
        }
    }

    public void OnUpdateFBUserInfoProperty(FBUserInfo property)
    {

    }

    public void OnUpdateFBUserItemProperty(FBUserItem property)
    {

    }
}
