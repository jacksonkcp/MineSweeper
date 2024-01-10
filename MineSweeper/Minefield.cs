using System;
using System.Collections.Generic;

namespace MineSweeper
{
    internal class Minefield
    {
        string[,] Answer; // Actual field i.e the answer
        uint MineSize;
        string[,] Field; // Field that is shown to user
        int NumberOfRevealedSquares = 0;
        Queue<Coordinate> queue = new Queue<Coordinate>();

        public Minefield(uint gridSize, uint mineSize)
        {
            Answer = new string[gridSize, gridSize];
            Field = new string[gridSize, gridSize];
            MineSize = mineSize;
            InitializeField();
            AddMinesToAnswer();
        }

        private void InitializeField()
        {
            for (int j = 0; j < Field.GetLength(0); j++)
            {
                for (int k = 0; k < Field.GetLength(1); k++) {
                    Field[j, k] = "_";
                }
            }
        }

        private void AddMinesToAnswer()
        {
            Random random = new Random();
            int mines = Convert.ToInt32(MineSize);

            while (mines-- > 0)
            {
                int row = random.Next(0, Answer.GetLength(0) - 1);
                int col = random.Next(0, Answer.GetLength(0) - 1);

                //Keep looking for a square that is not already a mine.
                while (Answer[row, col] == "x")
                {
                    row = random.Next(0, Answer.GetLength(0) - 1);
                    col = random.Next(0, Answer.GetLength(0) - 1);
                }

                Answer[row, col] = "x";
            }

            //Update all squares with their respective values i.e no. of adjacent mines.
            UpdateAnswer();
        }

        private void UpdateAnswer()
        {
            for (int j = 0; j < Answer.GetLength(0); j++)
            {
                for (int k = 0; k < Answer.GetLength(1); k++)
                {
                    // Ignore squares that are mines.
                    if (Answer[j, k] == "x")
                    {
                        continue;
                    }

                    int value = CountAdjMines(new Coordinate(j,k));

                    // Update square value.
                    Answer[j, k] = value.ToString();
                }
            }
        }

        private void ProcessQueue()
        {
            while (queue.Count > 0)
            {
                Coordinate coord = queue.Dequeue();
                //Reveal square if it is unrevealed
                if (IsUnrevealed(coord))
                {
                    Field[coord.Row, coord.Column] = Answer[coord.Row, coord.Column];
                    NumberOfRevealedSquares++;
                }

                // Adds all neighbors if it is a zero
                if (IsAZero(coord))
                {
                    EnqueueAllAdjacent(coord);
                }
            }
        }

        private void EnqueueAllAdjacent(Coordinate coord)
        {
            foreach (var displacement in Constants.AdjacentDisplacements)
            {
                Coordinate neighbor = new Coordinate(coord.Row + displacement.Row, coord.Column + displacement.Column);
                if (IsWithinBounds(neighbor) && IsUnrevealed(neighbor))
                {
                    Field[neighbor.Row, neighbor.Column] = Answer[neighbor.Row, neighbor.Column];
                    NumberOfRevealedSquares++;
                    queue.Enqueue(neighbor);
                }
            }
        }
 

        public int CountAdjMines(Coordinate coord)
        {
            int value = 0;

            foreach (var displacement in Constants.AdjacentDisplacements)
            {
                Coordinate neighbor = new Coordinate(coord.Row + displacement.Row, coord.Column + displacement.Column);
                if (IsAMine(neighbor))
                {
                    value++;
                }
            }


            return value;
        }

        private bool IsWithinBounds(Coordinate coord)
        {
            if (coord.Row >= 0 && coord.Row < Answer.GetLength(0) && coord.Column >= 0 && coord.Column < Answer.GetLength(1))
            {
                return true;
            }
            return false;
        }

        public bool IsAMine(Coordinate coord)
        {
            // Ensure the square is within the bounds of the field.
            if (IsWithinBounds(coord))
            {
                if (Answer[coord.Row, coord.Column] == "x")
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsAZero(Coordinate coord)
        {
            // Ensure the square is within the bounds of the field.
            if (IsWithinBounds(coord))
            {
                if (Answer[coord.Row, coord.Column] == "0")
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsUnrevealed(Coordinate coord)
        {
            if (IsWithinBounds(coord))
            {
                if (Field[coord.Row, coord.Column] == "_")
                {
                    return true;
                }
            }
            return false;
        }

        // Prints the answer
        public void PrintAnswer()
        {
            string line = " ";

            //Prints first line i.e column labels.
            for (int i = 1; i <= Answer.GetLength(1); i++)
            {
                line += " " + i;
            }

            Console.WriteLine(line);

            for (int j=0; j<Answer.GetLength(0); j++)
            {
                line = Constants.RowLabel[j].ToString();
                for (int k=0; k<Answer.GetLength(1); k++)
                {
                     line += " " + Answer[j, k].ToString();
                }

                Console.WriteLine(line);
            }
            Console.Write("\n");
        }

        public void PrintField()
        {
            string line = " ";

            //Prints first line i.e column labels. It starts from 1.
            for (int i = 1; i <= Field.GetLength(1); i++)
            {
                line += " " + i;
            }

            Console.WriteLine(line);

            for (int j = 0; j < Field.GetLength(0); j++)
            {
                line = Constants.RowLabel[j].ToString();
                for (int k = 0; k < Field.GetLength(1); k++)
                {
                    line += " " + Field[j, k].ToString();
                }

                Console.WriteLine(line);
            }

            Console.Write("\n");
        }

        public void RevealSquareAndAdjacentZeroes(Coordinate coord)
        {
            queue.Enqueue(coord);
            ProcessQueue();
        }

        public bool SuccessfulCompletion()
        {
            int leftover = Answer.Length - NumberOfRevealedSquares;

            // Only mines are left in the field. Game is completed.
            if (leftover == MineSize)
            {
                return true;
            }

            return false;
        }
    }
}
