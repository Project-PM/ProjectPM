using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public void LoadScene(SceneType type)
    {
        //Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    private string GetSceneName(SceneType type)
    {
        string name = System.Enum.GetName(typeof(SceneType), type);
        return name;
    }

    public void Clear()
    {
        //CurrentScene.Clear();
    }
}
