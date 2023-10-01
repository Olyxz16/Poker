using System.Numerics;

namespace GUISharp;


public struct Location {

    private Vector2 _bottomLeft;
    private Vector2 _topRight;

    public readonly Vector2 TopLeft => new(_bottomLeft.X, _topRight.Y);
    public readonly Vector2 BottomLeft => _bottomLeft;
    public readonly Vector2 TopRight => _topRight;
    public readonly Vector2 BottomRight => new(_topRight.X, _bottomLeft.Y);

    public readonly int MinX => (int)_bottomLeft.X;
    public readonly int MaxX => (int)_topRight.X;
    public readonly int MinY => (int)_bottomLeft.Y;
    public readonly int MaxY => (int)_topRight.Y;

    public readonly int SizeX => MaxX+1 - MinX;
    public readonly int SizeY => MaxY+1 - MinY;

    public Location(Vector2 bottomLeft, Vector2 topRight) {
        _bottomLeft = bottomLeft;
        _topRight = topRight;
    }

}