using GameLogic.Models;

namespace GameLogic
{
    public class GameState : IGameState
    {
        private GridValue[,]? Grid { get; set; }
        private Position? HumanPosition { get; set; } = new();
        private List<Position>? ExitPositions { get; set; } = new();
        private Position? CurrTargetPos { get; set; } = new();
        private List<Position>? ActivePositions { get; set; } = new();
        private List<Position>? VisitedPositions { get; set; } = new();
        private List<int> Results { get; set; } = new();

        public GameState(string filePath)
        {
            Grid = SetupGrid(filePath);
        }

        public int GetShortestPath()
        {
            try
            {
                for (int i = 0; i < ExitPositions?.Count; i++)
                {
                    VisitedPositions?.Clear();
                    CurrTargetPos = ExitPositions[i];
                    HumanPosition?.SetDistance(CurrTargetPos.Row, CurrTargetPos.Col);
                    ActivePositions?.Add(HumanPosition);

                    RunLoop();
                }

                return Results.Count == 0 ? 0 : Results.Min();
            }
            catch
            {
                return 0;
            }
        }

        private void RunLoop()
        {
            while (ActivePositions.Any())
            {
                var currentPos = ActivePositions.OrderBy(pos => pos.StepsDistance).First();

                if (currentPos.Row == CurrTargetPos?.Row && currentPos.Col == CurrTargetPos.Col)
                {
                    Results.Add(currentPos.Steps);
                    Grid[currentPos.Row, currentPos.Col] = GridValue.Obstacle;
                }

                VisitedPositions?.Add(currentPos);
                ActivePositions.Remove(currentPos);

                SetPossiblePositions(currentPos);
            }
        }

        private void SetPossiblePositions(Position currentPos)
        {
            var possiblePositions = GetPossiblePositions(currentPos, CurrTargetPos);

            foreach (var possiblePos in possiblePositions)
            {
                if (VisitedPositions.Any(pos => pos.Row == possiblePos.Row && pos.Col == possiblePos.Col))
                    continue;

                if (ActivePositions.Any(pos => pos.Row == possiblePos.Row && pos.Col == possiblePos.Col))
                {
                    var existingPos = ActivePositions.First(pos => pos.Row == possiblePos.Row && pos.Col == possiblePos.Col);
                    if (existingPos.StepsDistance > currentPos.StepsDistance)
                    {
                        ActivePositions.Remove(existingPos);
                        ActivePositions.Add(possiblePos);
                    }
                }
                else
                {
                    ActivePositions.Add(possiblePos);
                }
            }
        }

        public List<Position>? GetPossiblePositions(Position currPos, Position currExit)
        {
            var possiblePositions = new List<Position>()
            {
            new Position { Row = currPos.Row, Col = currPos.Col - 1, Previous = currPos, Steps = currPos.Steps + 1 },
            new Position { Row = currPos.Row, Col = currPos.Col + 1, Previous = currPos, Steps = currPos.Steps + 1 },
            new Position { Row = currPos.Row - 1, Col = currPos.Col, Previous = currPos, Steps = currPos.Steps + 1 },
            new Position { Row = currPos.Row + 1, Col = currPos.Col, Previous = currPos, Steps = currPos.Steps + 1 },
            };

            possiblePositions.ForEach(pos => pos.SetDistance(currExit.Row, currExit.Col));

            return possiblePositions
                    .Where(pos => pos.Row >= 0 && pos.Row <= Grid?.GetLength(0) - 1)
                    .Where(pos => pos.Col >= 0 && pos.Col <= Grid?.GetLength(1) - 1)
                    .Where(pos => Grid?[pos.Row, pos.Col] == GridValue.Empty || Grid?[pos.Row, pos.Col] == GridValue.Exit)
                    .ToList();
   
        }

        public GridValue[,]? SetupGrid(string filePath)
        {
            try
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
            catch
            {
                return null;
            }
        }
    }
}