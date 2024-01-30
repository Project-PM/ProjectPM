using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_TitleScene : UI_BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StartLoadAssets();

        return true;
    }

    void StartLoadAssets()
    {
        // PreLoad ���� ���� ��� ���ҽ��� �ε��ϰ� �ݹ��Լ��� �Ϸ� �뺸�� ����
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.Data.Init();

                // ���̵��� ������ ������Ʈ Ȱ��ȭ��Ű��?
            }
        });
    }
}
