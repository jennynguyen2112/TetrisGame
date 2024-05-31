using System;
using System.Windows;
using SplashKitSDK;

namespace TetrisGame
{
    public class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("Tetris",600,600);            
            TetrisGame game = new TetrisGame(20,20);
            while ( game.Quit != true )
            {   
                game.HandleInput();
                game.GameLoop();
                game.Draw();
                SplashKit.Delay(20);
            }
        }
    }
}
