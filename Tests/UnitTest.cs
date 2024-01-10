using MineSweeper;
using System.Net.Http.Headers;

namespace Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestGameCompletion()
        {
            Minefield minefield = new Minefield(1, 1);

            // Game should end when the only squares left are mines.
            Assert.AreEqual(true, minefield.SuccessfulCompletion());
        }

        [TestMethod]
        public void TestIfFieldGenerationIsCorrect()
        {
            Minefield minefield = new Minefield(2, 1); // It will always result in 3 squares with a value of 1 and one square with an 'x' aka a mine.
            int ones = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Coordinate coord = new Coordinate(i, j);
                    if (minefield.CountAdjMines(coord) == 1)
                    {
                        ones++;
                    }
                    else
                    {
                        if (!minefield.IsAMine(coord))
                        {
                            // It has to be a mine if it is not a 1 so this should not happen.
                            Assert.Fail();
                        }
                    }
                }
            }
            Assert.AreEqual(3, ones);
        }
    }
}