
// 네임스페이스나 스태틱 클래스로 묶을까 고민 중

/// <summary>
/// 해당 리소스의 확장자명을 기준으로 리소스 타입을 구분
/// </summary>
public enum ResourceType
{
    None,
    asset,
    prefab,
    controller,
    anim,
    mat,
    json,
}

public enum SceneType
{
    Unknown,
    Title,
    Lobby,
    Battle,
}

public enum LayerType
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Map = 3,
    Water = 4,
    UI = 5,
    Ground = 6,
}

// HARD CODING
