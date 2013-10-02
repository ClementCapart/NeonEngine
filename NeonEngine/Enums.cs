namespace NeonEngine
{
    public enum DrawLayer
    {
        None, Background0, Background1, Middleground2, Middleground3, Middleground4, Foreground5, Foreground6, HUD
    }

    public enum WallType
    {
        Solid, OneWay
    }

    public enum ShadowmapSize
    {
        Size128 = 6,
        Size256 = 7,
        Size512 = 8,
        Size1024 = 9,
        Size2048 = 10, 
        Size4096 = 11,
    }

    public enum SideDirection
    {
        Right, Down, Left, Up
    }

    public enum GraphicShapeType
    {
        Rectangle, Circle, Polygon
    }

    public enum AnimationShapeType
    {
        Rectangle, Circle
    }

    public enum BodyShapeType
    {
        Rectangle, Circle, Polygon
    }

    public enum ControllerState
    {
        Idle, Run, Jump, Drop
    }

    public enum MouseButton
    {
        LeftButton, RightButton, MiddleButton
    }

    public enum ControllerButton
    {
        A, B, X, Y, Start, Select, LeftShoulder, RightShoulder, LeftStick, RightStick
    }

    public enum ScrollingType
    {
        HorizontalScroll, VerticalScroll, FreeScroll
    }

    public enum DelayStatus
    {
        Passed,
        NotStarted,
        Valid
    }
}