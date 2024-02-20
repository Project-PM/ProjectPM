
public enum DirectionType
{
    Left, Right, Top, Bottom
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

/// <summary>
/// ���̾�̽��� ����Ǵ� ������ �׷� Ÿ��
/// </summary>
public enum FirebaseDataType
{
    UserInfo,
    UserItem,
}

public enum UserLoginType
{
    Guest,
    Google,
}