using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum EScene
    {
        Unknown,
        Title,
        Lobby,
        Battle,
    }

    public enum EUIEvent
    {
        Click,
        PointerDown,
        PointerUp,
        Drag,
    }


    public enum ESound
    {
        Bgm,
        Effect,
        Max,
    }

    public enum ELayer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Map = 3,
        Water = 4,
        UI = 5,
        Ground = 6,
    }

    public const int CAMERA_PROJECTION_SIZE = 12;

    // HARD CODING
    // public const float NAME = 100000;
}
