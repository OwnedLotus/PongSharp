using System.Numerics;

namespace PongSharp.models;

class Ball
{
    public Vector2 position;
    public ushort width;
    public float speed;
    public Vector2 heading;
    public bool movingUp;
    public bool movingLeft;

    public Ball(Vector2 pos, ushort w, float sp)
    {
        position = pos;
        width = w;
        speed = sp;

        Random rn = new Random();
        heading.X = rn.NextSingle();
        heading.Y = rn.NextSingle();
        Console.WriteLine($"Generated: {heading}");

        heading = Vector2.Normalize(heading);
        Console.WriteLine($"Normalized: {heading}");
    }

}
