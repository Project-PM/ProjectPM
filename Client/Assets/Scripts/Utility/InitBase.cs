using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBase : MonoBehaviour
{
    protected bool _init = false;

    /// <summary>
    /// Awake 시점에 최초 1번만 호출
    /// </summary>
    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    private void Awake()
    {
        Init();
    }
}
