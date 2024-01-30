using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton
{
    public void Awake()
    {
        OnAwakeInstance();
    }

    protected virtual void OnAwakeInstance()
    {

    }
}

public class Singleton<T> : Singleton where T : Singleton, new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
                instance.Awake();
            }

            return instance;
        }
    }
}
