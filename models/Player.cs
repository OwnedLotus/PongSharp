using Raylib_cs;
using System.Numerics;


namespace PongSharp.models;

class Player
{
    public ushort id;
    public string name = String.Empty;
    public Vector2 position;

    public ushort playerWidth;
    public ushort playerHeight;
    private Color playerColor;

    public ushort points = 0;

    public Player(ushort _id, string _name, Vector2 pos, ushort width, ushort height)
    {
        this.id = _id;
        this.name = _name;

        position = pos;
        playerWidth = width;
        playerHeight = height;

    }

    public void MovePlayer(float scale)
    {
        position.Y += (1 * scale);
    }
}
