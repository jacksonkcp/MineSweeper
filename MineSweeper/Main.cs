using System;

namespace MineSweeper
{
    public class MineSweeperGame
    {
        static uint GridSize;
        static uint MineSize;
        static Coordinate CurrentSquare;
        static public void Main(String[] args)
        {
            Console.WriteLine("Welcome to Minesweeper!");
 
            //Getting grid size from user
            GridInputRequest:
                Console.WriteLine("Enter the size of the grid (e.g. 4 for a 4x4 grid):");
                String gridInput = Console.ReadLine();
                if (UInt32.TryParse(gridInput, out GridSize))
                {
                    if (GridSize < 2)
                    {
                        Console.WriteLine("Minimum size of grid is 2.");
                        goto GridInputRequest;
                    }
                    if (GridSize > 10)
                    {
                        Console.WriteLine("Maximum size of grid is 10");
                        goto GridInputRequest;
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect input.");
                    goto GridInputRequest;
                }

                //Max number of mines is 35% of total squares (gridInt^2)
                double maxMine = Math.Floor(0.35 * GridSize * GridSize);

            //Getting number of mines from user
            MineInputRequest:
                Console.WriteLine("Enter the number of mines to place on the grid (maximum is 35% of the total squares):");
                string mineInput = Console.ReadLine();

                if (UInt32.TryParse(mineInput, out MineSize))
                {
                    if (MineSize < 1)
                    {
                        Console.WriteLine("There must be at least 1 mine.");
                        goto MineInputRequest;
                    }
                    if (MineSize > maxMine)
                    {
                        Console.WriteLine($"Maximum number is 35% of total squares: {maxMine}");
                        goto MineInputRequest;
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect input.");
                    goto MineInputRequest;
                }

            Minefield minefield = new Minefield(GridSize, MineSize);

            // Prints the initial field with all squares hidden
            Console.WriteLine("Here is your minefield:");
            minefield.PrintField();

        SquareRequest:
            if (minefield.SuccessfulCompletion())
            {
                Console.WriteLine("Congratulations, you have won the game!");
                Gameover();

                // Restarts the game.
                goto GridInputRequest;
            }
            Console.Write("Select a square to reveal(e.g.A1): ");
            string input = Console.ReadLine();
            
            if (!IsValidSquareInput(input))
            {
                Console.WriteLine($"Invalid square: {input}");
                goto SquareRequest;
            }

            // Game over
            if (minefield.IsAMine(CurrentSquare))
            {
                Console.WriteLine("Oh no, you detonated a mine! Game over.\n");
                minefield.PrintAnswer();
                Gameover();


                // Restarts the game.
                goto GridInputRequest;
            }

            // Not a mine
            Console.WriteLine($"This square contains {minefield.CountAdjMines(CurrentSquare)} adjacent mines.\n");
            minefield.RevealSquareAndAdjacentZeroes(CurrentSquare);

            Console.WriteLine("Here is your updated minefield:");
            minefield.PrintField();
            goto SquareRequest;
        }

        private static bool IsValidSquareInput(string input)
        {
            // Ensure row is valid. Ensure there are at least two characters in the input.
            if (String.IsNullOrEmpty(input) || !Constants.RowLabel.Contains(input[0].ToString()) || input.Length < 2)
            {
                return false;
            }

            int row = Constants.RowLabel.IndexOf(input[0].ToString());

            // Row is out of range
            if (row >= GridSize)
            {
                return false;
            }

            int col;
            string colString = input.Substring(1);

            // Invalid column character
            if (!Int32.TryParse(colString, out col))
            {
                return false;
            }
            else
            {
                // Out of range column number
                if (col < 1 || col > GridSize)
                {
                    return false;
                }
            }

            // This is a valid square. Minus 1 from column to make it 0-based. Store the valid parsed input for later use.
            CurrentSquare = new Coordinate(row, col - 1);
            return true;
        }

        private static void Gameover()
        {
            Console.WriteLine("Press any key to play again or N to exit the game");
            string input = Console.ReadLine();

            //Exits the game if user presses n or N
            if (input.ToUpper() == "N")
            {
                System.Environment.Exit(0);
            }
        }
    }
}

