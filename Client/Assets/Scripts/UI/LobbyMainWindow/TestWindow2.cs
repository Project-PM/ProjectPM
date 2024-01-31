using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestWindow2 : BaseWindowUI
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;



        return true;
    }

    protected override void SetWindowUIType()
    {
        WindowUIType = WindowUIType.TestWindow2;
    }

    
}
