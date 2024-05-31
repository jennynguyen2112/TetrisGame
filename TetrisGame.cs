using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using SplashKitSDK;

namespace TetrisGame
{
    public class TetrisGame
    {
        private readonly int rows;
        private readonly int columns;
        private int[,] gameBoard;
        public SplashKitSDK.Timer myTimer;
        public Block currentBlock { get; private set; }
        private int score;
        public bool Quit {get; private set; }   

        public TetrisGame(int gameRows, int gameColumns)
        {
            rows = gameRows;
            columns = gameColumns;
            gameBoard = new int[rows, columns];
            currentBlock = Block.GenerateRandomBlock(columns);
            myTimer = new SplashKitSDK.Timer("My Timer");
            myTimer.Start();
            score = 0;
        }

        public void GameLoop()
        {
           if (myTimer.Ticks > 1000)
            {
                MoveBlockDown();
                myTimer.Reset();
                myTimer.Start();
            }
            CheckForCompletedRows();
        }

        public void HandleInput()
        {
            SplashKit.ProcessEvents();
            if (SplashKit.KeyDown(KeyCode.EscapeKey)) Quit = true;
            else if ( SplashKit.KeyDown(KeyCode.UpKey) ) currentBlock.Rotate(gameBoard);
            else if ( SplashKit.KeyDown(KeyCode.DownKey) ) MoveBlockDown();
            else if ( SplashKit.KeyDown(KeyCode.LeftKey) ) currentBlock.MoveLeft(gameBoard);
            else if ( SplashKit.KeyDown(KeyCode.RightKey) ) currentBlock.MoveRight(gameBoard); 
        }


        public void Draw()
        {
            int cellSize = 30;
            SplashKit.ClearScreen(SplashKitSDK.Color.White);
            DrawScore();
            // Draw the game board
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    SplashKit.DrawRectangle(SplashKitSDK.Color.Gray, c * cellSize, r * cellSize, cellSize, cellSize);
                    if (gameBoard[r, c] == 1)
                    {
                        SplashKit.FillRectangle(SplashKitSDK.Color.Blue, c * cellSize, r * cellSize, cellSize, cellSize);
                        SplashKit.DrawRectangle(SplashKitSDK.Color.Black, c * cellSize, r * cellSize, cellSize, cellSize);
                    }
                }
                
                // Draw the current block
                foreach (var cell in currentBlock.Cells)
                {
                    SplashKit.FillRectangle(SplashKitSDK.Color.Red, cell.Item2 * cellSize, cell.Item1 * cellSize, cellSize, cellSize);
                    SplashKit.DrawRectangle(SplashKitSDK.Color.Black, cell.Item2 * cellSize, cell.Item1 * cellSize, cellSize, cellSize);
                }
                
                // Refresh the screen to show the new drawings
                SplashKit.RefreshScreen();
            }
        }

        private void MoveBlockDown()
        {
            if (!currentBlock.MoveDown(gameBoard))
            {
                LockBlock();
                currentBlock = Block.GenerateRandomBlock(columns);
            }
        }


        private void LockBlock()
        {
            foreach (var cell in currentBlock.Cells)
            {
                gameBoard[cell.Item1, cell.Item2] = 1;
            }
        }

        private void CheckForCompletedRows()
        {
            for (int r = 0; r < rows; r++)
            {
                bool rowComplete = true;
                for (int c = 0; c < columns; c++)
                {
                    if (gameBoard[r, c] == 0)
                    {
                        rowComplete = false;
                        break;
                    }
                }

                if (rowComplete)
                {
                    RemoveRow(r);
                    score += 100; // Increment score for each completed row
                }
            }
        }

        private void RemoveRow(int row)
        {
            for (int r = row; r > 0; r--)
            {
                for (int c = 0; c < columns; c++)
                {
                    gameBoard[r, c] = gameBoard[r - 1, c];
                }
            }

            // Clear the top row
            for (int c = 0; c < columns; c++)
            {
                gameBoard[0, c] = 0;
            }
        }
        public void DrawScore()
        {
            SplashKit.DrawText($"SCORE: {score}", SplashKitSDK.Color.HotPink, 10, 10);

        }

    }
}

    