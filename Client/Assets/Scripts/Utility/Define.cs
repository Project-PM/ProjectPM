
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

public enum CharacterType
{
    Red = 0,
    Blue = 1,
    Green = 2,
    Max = 3,
}

public enum CharacterGearType
{
    Cap = 0,
    TopClothes = 1,
    BottomClothes = 2,
    Boots = 3,
    Accessories = 4,
    Weapon = 5,
    Max = 6,
}

public enum PlayerPrefKeyType
{
    UserToken,
}