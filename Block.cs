using System;
using System.Collections.Generic;

namespace TetrisGame
{
    public class Block
    {
        public List<(int, int)> Cells { get; private set; }

        public Block(List<(int, int)> cells)
        {
            Cells = cells;
        }

        public static Block GenerateRandomBlock(int columns)
        {
            Random rand = new Random();
            int blockType = rand.Next(0, 7); // 7 types of Tetris blocks

            switch (blockType)
            {
                case 0: // I block
                    return new Block(new List<(int, int)> { (0, columns / 2 - 1), (0, columns / 2), (0, columns / 2 + 1), (0, columns / 2 + 2) });
                case 1: // O block
                    return new Block(new List<(int, int)> { (0, columns / 2), (0, columns / 2 + 1), (1, columns / 2), (1, columns / 2 + 1) });
                case 2: // T block
                    return new Block(new List<(int, int)> { (0, columns / 2), (1, columns / 2 - 1), (1, columns / 2), (1, columns / 2 + 1) });
                case 3: // S block
                    return new Block(new List<(int, int)> { (0, columns / 2), (0, columns / 2 + 1), (1, columns / 2 - 1), (1, columns / 2) });
                case 4: // Z block
                    return new Block(new List<(int, int)> { (0, columns / 2 - 1), (0, columns / 2), (1, columns / 2), (1, columns / 2 + 1) });
                case 5: // J block
                    return new Block(new List<(int, int)> { (0, columns / 2 - 1), (1, columns / 2 - 1), (1, columns / 2), (1, columns / 2 + 1) });
                case 6: // L block
                    return new Block(new List<(int, int)> { (0, columns / 2 + 1), (1, columns / 2 - 1), (1, columns / 2), (1, columns / 2 + 1) });
                default:
                    throw new Exception("Invalid block type");
            }
        }

        public bool MoveDown(int[,] gameBoard)
        {
            if (CanMove(gameBoard, 1, 0))
            {
                for (int i = 0; i < Cells.Count; i++)
                {
                    Cells[i] = (Cells[i].Item1 + 1, Cells[i].Item2);
                }
                return true;
            }
            return false;
        }

        public void MoveLeft(int[,] gameBoard)
        {
            if (CanMove(gameBoard, 0, -1))
            {
                for (int i = 0; i < Cells.Count; i++)
                {
                    Cells[i] = (Cells[i].Item1, Cells[i].Item2 - 1);
                }
            }
        }

        public void MoveRight(int[,] gameBoard)
        {
            if (CanMove(gameBoard, 0, 1))
            {
                for (int i = 0; i < Cells.Count; i++)
                {
                    Cells[i] = (Cells[i].Item1, Cells[i].Item2 + 1);
                }
            }
        }
        //Method to rotate the block 90 degrees
        public void Rotate(int[,] gameBoard)
        {
            int centerX = Cells[0].Item1;
            int centerY = Cells[0].Item2;

            List<(int, int)> newCells = new List<(int, int)>();
            foreach (var cell in Cells)
            {
                int newX = centerX - (cell.Item2 - centerY);
                int newY = centerY + (cell.Item1 - centerX);
                newCells.Add((newX, newY));
            }

            if (CanMove(gameBoard, newCells))
            {
                Cells = newCells;
            }
        }
        //CanMove method to check if the block can move deltaX or deltaY space  
        public bool CanMove(int[,] gameBoard, int deltaX, int deltaY)
        {
            List<(int, int)> newCells = new List<(int, int)>();
            foreach (var cell in Cells)
            {
                newCells.Add((cell.Item1 + deltaX, cell.Item2 + deltaY));
            }
        //using method overloading
            return CanMove(gameBoard, newCells);
        }

        public bool CanMove(int[,] gameBoard, List<(int, int)> newCells)
        {
            foreach (var cell in newCells)
            {
                if (cell.Item1 < 0 || cell.Item1 >= gameBoard.GetLength(0) ||
                    cell.Item2 < 0 || cell.Item2 >= gameBoard.GetLength(1) ||
                    gameBoard[cell.Item1, cell.Item2] == 1)
                {
                    return false;
                }
            }
            return true;
        }


    }
}
