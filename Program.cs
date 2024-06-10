using Raylib_cs;
using PongSharp.models;
using System.Numerics;

namespace HelloWorld;

class Program
{
    public static void Main()
    {
        //TODO! Bounds checking on the paddle collision
        //Bounds checking on the paddle movement
        //add points
        //add gamestates
        //add ball speed modifier

        Raylib.SetTargetFPS(60);

        var boardSize = new Vector2(800, 480);
        ushort playerWidth = 10;
        ushort playerHeight = 50;
        ushort boardMargin = 30;
        ushort ballWidth = 15;

        Raylib.InitWindow((int)boardSize.X, (int)boardSize.Y, "Hello World");

        var p1spawn = new Vector2(boardMargin * 2, boardSize.Y / 2);
        var p2spawn = new Vector2(boardSize.X - boardMargin * 2, boardSize.Y / 2);

        float movementSpeed = 1f;
        float ballSpeed = 1f;

        var players = (new Player(0, "Player1", p1spawn, playerWidth, playerHeight), new Player(1, "Player2", p2spawn, playerWidth, playerHeight));



        var gameBoard = new GameBoard(boardSize, players, boardMargin, movementSpeed, ballSpeed, ballWidth);



        while (!Raylib.WindowShouldClose())
        {
            gameBoard.UpdateBoard();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);


            gameBoard.DrawBoard();
            gameBoard.DrawPlayers();

            Raylib.DrawFPS(10, 10);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
