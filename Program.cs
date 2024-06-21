using Raylib_cs;

using PongSharp.models;
using System.Numerics;

namespace HelloWorld;

class Program
{
    public static void Main()
    {

        // Current issue, inconsistant collisions on paddles and fast misses
        Raylib.SetTargetFPS(60);

        var boardSize = new Vector2i(800, 480);
        ushort playerWidth = 10;
        ushort playerHeight = 50;
        ushort boardMargin = 30;
        ushort ballWidth = 15;

        Raylib.InitWindow((int)boardSize.X, (int)boardSize.Y, "Hello World");

        var p1spawn = new Vector2(boardMargin * 2, boardSize.Y / 2);
        var p2spawn = new Vector2(boardSize.X - boardMargin * 2, boardSize.Y / 2);

        float movementSpeed = 1f;
        float ballSpeed = 1f;

        var players = (new Player(0, "Player1", p1spawn,
                playerWidth, playerHeight), new Player(1, "Player2", p2spawn, playerWidth, playerHeight));

        var gameBoard = new GameBoard(boardSize, players,
                boardMargin, movementSpeed, ballSpeed, ballWidth);

        while (!Raylib.WindowShouldClose())
        {
            switch (gameBoard.state)
            {
                case GameState.Startup:
                    RunStartup();

                    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                    {
                        gameBoard.state = GameState.Playing;
                    }
                    break;

                case GameState.Playing:
                    gameBoard.UpdateBoard();
                    break;

                case GameState.GameOver:
                    RunGameOver();
                    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                    {
                        gameBoard.state = GameState.Startup;
                    }
                    if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                    {
                        Environment.Exit(0);
                    }
                    break;
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            Raylib.DrawFPS(10, 10);

            gameBoard.DrawBoard();
            gameBoard.DrawPlayers();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();

        void RunStartup()
        {
            Raylib.DrawText("Play Pong -- Press Enter", 20, 20, 40, Color.Black);
        }

        void RunGameOver()
        {
            Raylib.DrawText("GAME OVER -- Press Enter to play again", 20, 20, 20, Color.Black);
            Raylib.DrawText("Press Escape to quit the game", 20, 40, 20, Color.Black);
        }
    }

}
