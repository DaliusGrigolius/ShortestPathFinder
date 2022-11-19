using GameLogic.Models;

namespace GameLogic
{
    public class GameState : IGameState
    {
        private GridValue[,]? Grid { get; set; }
        private Position? HumanPosition { get; set; } = new();
        private List<Position>? ExitPositions { get; set; } = new();

        public GameState(string filePath)
        {
            Grid = SetupGrid(filePath);
        }

        public int GetShortestPath()
        {
            List<int> Results = new();

            for (int i = 0; i < ExitPositions?.Count; i++)
            {
                var currTargetPos = ExitPositions[i];

                HumanPosition?.SetDistance(currTargetPos.Row, currTargetPos.Col);

                var activePositions = new List<Position>();
                activePositions.Add(HumanPosition);
                var visitedPositions = new List<Position>();

                while (activePositions.Any())
                {
                    var currentPos = activePositions.OrderBy(pos => pos.StepsDistance).First();

                    if (currentPos.Row == currTargetPos.Row && currentPos.Col == currTargetPos.Col)
                    {
                        Results.Add(currentPos.Steps);
                        Grid[currentPos.Row, currentPos.Col] = GridValue.Obstacle;
                    }

                    visitedPositions.Add(currentPos);
                    activePositions.Remove(currentPos);

                    var possiblePositions = GetPossiblePositions(currentPos, currTargetPos);

                    foreach (var possiblePos in possiblePositions)
                    {
                        if (visitedPositions.Any(pos => pos.Row == possiblePos.Row && pos.Col == possiblePos.Col))
                            continue;

                        if (activePositions.Any(pos => pos.Row == possiblePos.Row && pos.Col == possiblePos.Col))
                        {
                            var existingPos = activePositions.First(pos => pos.Row == possiblePos.Row && pos.Col == possiblePos.Col);
                            if (existingPos.StepsDistance > currentPos.StepsDistance)
                            {
                                activePositions.Remove(existingPos);
                                activePositions.Add(possiblePos);
                            }
                        }
                        else
                        {
                            activePositions.Add(possiblePos);
                        }
                    }
                }
            }

            return Results.Count == 0 ? 0 : Results.Min();
        }

        private List<Position> GetPossiblePositions(Position currPos, Position currExit)
        {
            var possiblePositions = new List<Position>()
            {
                new Position { Row = currPos.Row, Col = currPos.Col - 1, Previous = currPos, Steps = currPos.Steps + 1 },
                new Position { Row = currPos.Row, Col = currPos.Col + 1, Previous = currPos, Steps = currPos.Steps + 1 },
                new Position { Row = currPos.Row - 1, Col = currPos.Col, Previous = currPos, Steps = currPos.Steps + 1 },
                new Position { Row = currPos.Row + 1, Col = currPos.Col, Previous = currPos, Steps = currPos.Steps + 1 },
            };

            possiblePositions.ForEach(pos => pos.SetDistance(currExit.Row, currExit.Col));

            var maxX = Grid?.GetLength(0) - 1;
            var maxY = Grid?.GetLength(1) - 1;

            return possiblePositions
                    .Where(pos => pos.Row >= 0 && pos.Row <= maxX)
                    .Where(pos => pos.Col >= 0 && pos.Col <= maxY)
                    .Where(pos => Grid?[pos.Row, pos.Col] == GridValue.Empty || Grid?[pos.Row, pos.Col] == GridValue.Exit)
                    .ToList();
        }

        private GridValue[,] SetupGrid(string filePath)
        {
            string[] map = File.ReadAllLines(filePath);
            GridValue[,] grid = new GridValue[map.Length, map[0].Length];

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[0].Length; col++)
                {
                    if ((row == 0 || col == 0
                        || row == map.Length - 1 || col == map[0].Length - 1)
                        && map[row][col] == ' ')
                    {
                        grid[row, col] = GridValue.Exit;
                        ExitPositions?.Add(new Position(row, col));
                    }
                    else if (map[row][col] != ' ' && map[row][col] != 'X')
                    {
                        grid[row, col] = GridValue.Obstacle;
                    }
                    else if (map[row][col] == 'X')
                    {
                        grid[row, col] = GridValue.Human;
                        HumanPosition = new Position(row, col);
                    }
                    else
                    {
                        grid[row, col] = GridValue.Empty;
                    }
                }
            }

            return grid;
        }
    }
}