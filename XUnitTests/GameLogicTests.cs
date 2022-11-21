using GameLogic;
using GameLogic.Models;

namespace XUnitTests
{
    public class GameLogicTests
    {
        [Fact]
        public void GetShortestPath_FileExist_ReturnsCorrectResult()
        {
            var path = @"..\..\..\..\Repo\TestData\map1.txt";
            GameState gs = new GameState(path);

            var actual = gs.GetShortestPath();
            var expected = 4;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetShortestPath_FileDoesntExistOrIsCorrupted_ReturnsZero()
        {
            var path = @"..\..\..\..\Repo\TestData\noMap.txt";
            GameState gs = new GameState(path);

            var actual = gs.GetShortestPath();
            var expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetupGrid_FileDoesntExist_ReturnsZero()
        {
            var path = @"..\..\..\..\Repo\TestData\noMap.txt";
            GameState gs = new GameState(path);

            var actual = gs.SetupGrid(path);
            GridValue[,] expected = null;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetupGrid_FileExists_ReturnsGridValueType()
        {
            var path = @"..\..\..\..\Repo\TestData\map2.txt";
            GameState gs = new GameState(path);

            var actual = gs.SetupGrid(path);

            Assert.IsType<GridValue[,]>(actual);
        }

        [Fact]
        public void GetPossiblePositions_ThereIsValidPositions_ReturnsValidPositions()
        {
            var path = @"..\..\..\..\Repo\TestData\testMap.txt";
            GameState gs = new GameState(path);
            var grid = gs.SetupGrid(path);
            Position currentPos = new(2, 1);
            Position targetPos = new(3,2);

            List<Position> actual = gs.GetPossiblePositions(currentPos, targetPos);
            var expected = 2;

            Assert.Equal(expected, actual.Count);
        }

        [Fact]
        public void GetPossiblePositions_ThereIsNoValidPositions_ReturnsEmptyList()
        {
            var path = @"..\..\..\..\Repo\TestData\testMap1.txt";
            GameState gs = new GameState(path);
            var grid = gs.SetupGrid(path);
            Position currentPos = new(2, 1);
            Position targetPos = new(3, 2);

            List<Position> actual = gs.GetPossiblePositions(currentPos, targetPos);
            var expected = 0;

            Assert.Equal(expected, actual.Count);
        }
    }
}