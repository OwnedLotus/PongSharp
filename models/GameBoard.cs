using Raylib_cs;
using System.Numerics;

namespace PongSharp.models;

enum GameState
{
    Playing,
    GameOver,
    Startup
}

class GameBoard
{
    private bool isSinglePlayer = true;
    public GameState state;

    private (Vector2i, Vector2i) boardSize;

    private uint xWallLen;
    private uint yWallLen;
    private Wall northWall;
    private Wall southWall;
    private Wall eastWall;
    private Wall westWall;

    private ushort margin;

    private List<Player> Players;
    private Vector2 player1Spawn;
    private Vector2 player2Spawn;
    private float playerSpeed = 40f;
    private Ball ball;

    public GameBoard(Vector2i size, (Player, Player) players, ushort m, float ps, float bs, ushort ballWidth)
    {

        Players = new List<Player>()
        {
            players.Item1,
            players.Item2
        };

        margin = m;
        playerSpeed = ps;


        boardSize = (new Vector2i(0 + margin, 0 + margin), new Vector2i(size.X - margin, size.Y - margin));

        // position is naturally the average of the size of the board not offset by using boardSize which would be offset
        ball = new Ball(new Vector2(size.X / 2, size.Y / 2), ballWidth, bs);

        northWall = new(new Vector2i(boardSize.Item1.X, boardSize.Item1.Y), new Vector2i(boardSize.Item2.X, boardSize.Item1.Y));
        //Console.WriteLine($"North Wall Y: {northWall.yEnd}");
        southWall = new(new Vector2i(boardSize.Item1.X, boardSize.Item2.Y), new Vector2i(boardSize.Item2.X, boardSize.Item2.Y));
        //Console.WriteLine($"South Wall Y: {southWall.yEnd}");
        eastWall = new(new Vector2i(boardSize.Item2.X, boardSize.Item1.Y), new Vector2i(boardSize.Item2.X, boardSize.Item2.Y));
        //Console.WriteLine($"East Wall X: {eastWall.xEnd}");
        westWall = new(new Vector2i(boardSize.Item1.X, boardSize.Item1.Y), new Vector2i(boardSize.Item1.X, boardSize.Item2.Y));
        //Console.WriteLine($"West Wall X: {westWall.xEnd}");

        state = GameState.Startup;
    }


    public void DrawBoard()
    {
        if (state != GameState.Playing)
            return;

        // upper horizontal line
        Raylib.DrawLine(northWall.start.X, northWall.start.Y, northWall.end.X, northWall.end.Y, Color.Black);

        // lower horizontal line
        Raylib.DrawLine(southWall.start.X, southWall.start.Y, southWall.end.X, southWall.end.Y, Color.Black);

        // left vertical line
        Raylib.DrawLine(eastWall.start.X, eastWall.start.Y, eastWall.end.X, eastWall.end.Y, Color.Black);

        // Right vertical line
        Raylib.DrawLine(westWall.start.X, westWall.start.Y, westWall.end.X, westWall.end.Y, Color.Black);
    }

    public void DrawPlayers()
    {
        if (state != GameState.Playing)
            return;

        Raylib.DrawText($"Ball Position: {(int)ball.position.X}, {(int)ball.position.Y}", boardSize.Item2.X / 2, 10, 20, Color.Black);

        foreach (var player in Players)
        {
            Raylib.DrawRectangle((int)player.position.X, (int)player.position.Y, player.playerWidth, player.playerHeight, Color.Gray);
        }

        Raylib.DrawRectangle((int)ball.position.X, (int)ball.position.Y, ball.width, ball.width, Color.Black);
    }



    // takes in the input from both players at the same time
    // the first int is the player 1 controlled by w/s
    // the second int is the player 2 controlled by up/down
    public void UpdateBoard()
    {
        int player1XPos = (int)(Players[0].position.X + Players[0].playerWidth);
        int player2XPos = (int)(Players[1].position.X - Players[1].playerWidth);
        int player1YPos = (int)(Players[0].position.Y);
        int player2YPos = (int)(Players[1].position.Y);


        if (Raylib.IsKeyDown(KeyboardKey.W)) Players[0].position.Y -= 1 * playerSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.S)) Players[0].position.Y += 1 * playerSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.Up)) Players[1].position.Y -= 1 * playerSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.Down)) Players[1].position.Y += 1 * playerSpeed;

        int ballPositionX = (int)ball.position.X;
        int ballPositionY = (int)ball.position.Y;


        if (ballPositionY == northWall.end.Y)
        {
            Console.WriteLine($"Ball Current Position: {ball.position}");
            ball.heading.Y = -ball.heading.Y;
            Console.WriteLine($"New Heading: {ball.heading}");

        }
        else if (ballPositionY + ball.width == southWall.end.Y)
        {
            Console.WriteLine($"Ball Current Position: {ball.position}");
            ball.heading.Y = -ball.heading.Y;
            Console.WriteLine($"New Heading: {ball.heading}");
        }

        if (ballPositionX + ball.width == westWall.end.X)
        {
            Console.WriteLine($"Ball Current Position: {ball.position}");
            ball.heading.X = -ball.heading.X;
            Console.WriteLine($"New Heading: {ball.heading}");
        }
        else if (ballPositionX == eastWall.end.X)
        {
            Console.WriteLine($"Ball Current Position: {ball.position}");
            ball.heading.X = -ball.heading.X;
            Console.WriteLine($"New Heading: {ball.heading}");
        }
        if (ballPositionX == player1XPos)
        {
            Console.WriteLine("Player 1 Paddle hit");
            ball.heading.X = -ball.heading.X;
        }
        else if (ballPositionX == player2XPos)
        {
            Console.WriteLine("Player 2 Paddle hit");
            ball.heading.X = -ball.heading.X;
        }


        // ball will follow the heading 
        ball.position += (ball.heading * ball.speed);



    }
}

public record Wall(Vector2i start, Vector2i end);
public record Vector2i(int X, int Y);
