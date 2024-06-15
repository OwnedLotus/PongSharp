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


    private Vector2 boardSize;

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

    public GameBoard(Vector2 size, (Player, Player) players, ushort m, float ps, float bs, ushort ballWidth)
    {
        boardSize = size;

        Players = new List<Player>()
        {
            players.Item1,
            players.Item2
        };

        margin = m;
        playerSpeed = ps;

        ball = new Ball(boardSize / 2, ballWidth, bs);

        northWall = new Wall(0 + margin, 0 + margin, (int)boardSize.X - margin, 0 + margin);
        //Console.WriteLine($"North Wall Y: {northWall.yEnd}");
        southWall = new Wall(0 + margin, (int)boardSize.Y - margin, (int)boardSize.X - margin, (int)boardSize.Y - margin);
        //Console.WriteLine($"South Wall Y: {southWall.yEnd}");
        eastWall = new Wall(0 + margin, 0 + margin, 0 + margin, (int)boardSize.Y - margin);
        //Console.WriteLine($"East Wall X: {eastWall.xEnd}");
        westWall = new Wall((int)boardSize.X - margin, 0 + margin, (int)boardSize.X - margin, (int)boardSize.Y - margin);
        //Console.WriteLine($"West Wall X: {westWall.xEnd}");

        state = GameState.Startup;
    }


    public void DrawBoard()
    {
        if (state != GameState.Playing)
            return;

        // upper horizontal line
        Raylib.DrawLine(northWall.xStart, northWall.yStart, northWall.xEnd, northWall.yEnd, Color.Black);
        // lower horizontal line
        Raylib.DrawLine(southWall.xStart, southWall.yStart, southWall.xEnd, southWall.yEnd, Color.Black);

        // left vertical line
        Raylib.DrawLine(eastWall.xStart, eastWall.yStart, eastWall.xEnd, eastWall.yEnd, Color.Black);

        // Right vertical line
        Raylib.DrawLine(westWall.xStart, westWall.yStart, westWall.xEnd, westWall.yEnd, Color.Black);
    }

    public void DrawPlayers()
    {
        if (state != GameState.Playing)
            return;

        Raylib.DrawText($"Ball Position: {(int)ball.position.X}, {(int)ball.position.Y}", (int)boardSize.X / 2, 10, 20, Color.Black);

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


        if (ballPositionY == northWall.yEnd)
        {
            Console.WriteLine($"Ball Current Position: {ball.position}");
            ball.heading.Y = -ball.heading.Y;
            Console.WriteLine($"New Heading: {ball.heading}");

        }
        else if (ballPositionY + ball.width == southWall.yEnd)
        {
            Console.WriteLine($"Ball Current Position: {ball.position}");
            ball.heading.Y = -ball.heading.Y;
            Console.WriteLine($"New Heading: {ball.heading}");
        }

        if (ballPositionX + ball.width == westWall.xEnd)
        {
            Console.WriteLine($"Ball Current Position: {ball.position}");
            ball.heading.X = -ball.heading.X;
            Console.WriteLine($"New Heading: {ball.heading}");
        }
        else if (ballPositionX == eastWall.xEnd)
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

struct Wall
{
    public int xStart;
    public int yStart;
    public int xEnd;
    public int yEnd;

    public Wall(int x1, int y1, int x2, int y2)
    {
        xStart = x1;
        xEnd = x2;
        yStart = y1;
        yEnd = y2;
    }
}
