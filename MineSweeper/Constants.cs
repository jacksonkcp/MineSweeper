using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    internal class Constants
    {
        public static readonly List<string> RowLabel = new List<string>
        {
            // 10 letters for the row labels since the max grid size is 10.
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J"
        };

        public static readonly List<Coordinate> AdjacentDisplacements = new List<Coordinate>
        {
            // The displacements for the 8 adjacent squares around a square.
            new Coordinate(-1,-1),
            new Coordinate(1,1),
            new Coordinate(1,0),
            new Coordinate(0,1),
            new Coordinate(-1,0),
            new Coordinate(0,-1),
            new Coordinate(1,-1),
            new Coordinate(-1,1),
        };
    }
    internal class Coordinate
    {
        public Coordinate(int row, int col)
        {
            this.Row = row;
            this.Column = col;
        }

        public int Row { get; set; }
        public int Column { get; set; }
    }
}
