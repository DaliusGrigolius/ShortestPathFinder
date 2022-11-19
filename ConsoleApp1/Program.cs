
namespace PathfindingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            1. Map
            2. start position
            3. end position
            4. set distance to end pos
            5. empty position list <active>
            6. empty position list <visited>
            */

            List<string> map = new List<string>
            {
                "11111111111",
                "1     1   1",
                "1 1 1 111 1",
                "1 1 1     1",
                "1 1 1 11111",
                "1   1X    1",
                "1 1111111 1",
                "1   1     1",
                "111 1 111 1",
                "1   1 1   1",
                "11 11 11111"
            };
           
            var start = new Position();
            start.Y = map.FindIndex(row => row.Contains('X')); // index in row
            start.X = map[start.Y].IndexOf("X");               // row index

            var target = new Position();
            target.Y = map.FindIndex(row => row.Contains('B')); // index in row
            target.X = map[target.Y].IndexOf("B");              // row index

            start.SetDistance(target.X, target.Y);

            var activePos = new List<Position>();
            activePos.Add(start); 
            var visitedPos = new List<Position>();

            while (activePos.Any())
            {
                var current = activePos.OrderBy(TILE => TILE.StepsDistance).First();

                if (current.X == target.X && current.Y == target.Y)
                {
                    Console.WriteLine(current.Steps);
                    return;
                }

                visitedPos.Add(current);
                activePos.Remove(current);

                var possiblePositions = GetPossiblePositions(map, current, target);

                foreach (var possiblePosition in possiblePositions)
                {
                    if (visitedPos.Any(pos => pos.X == possiblePosition.X && pos.Y == possiblePosition.Y))
                        continue;

                    if (activePos.Any(pos => pos.X == possiblePosition.X && pos.Y == possiblePosition.Y)) 
                    {
                        var existingPos = activePos.First(pos => pos.X == possiblePosition.X && pos.Y == possiblePosition.Y);
                        if (existingPos.StepsDistance > current.StepsDistance)
                        {
                            activePos.Remove(existingPos);
                            activePos.Add(possiblePosition);
                        }
                    }
                    else
                    {
                        activePos.Add(possiblePosition);
                    }
                }
            }

            Console.WriteLine("No Path Found!");
        }

        private static List<Position> GetPossiblePositions(List<string> map, Position currentTile, Position targetTile)
        {
            var possiblePositions = new List<Position>()
            {
                new Position { X = currentTile.X, Y = currentTile.Y - 1, Previous = currentTile, Steps = currentTile.Steps + 1 },
                new Position { X = currentTile.X, Y = currentTile.Y + 1, Previous = currentTile, Steps = currentTile.Steps + 1 },
                new Position { X = currentTile.X - 1, Y = currentTile.Y, Previous = currentTile, Steps = currentTile.Steps + 1 },
                new Position { X = currentTile.X + 1, Y = currentTile.Y, Previous = currentTile, Steps = currentTile.Steps + 1 },
            };

            possiblePositions.ForEach(pos => pos.SetDistance(targetTile.X, targetTile.Y));

            var maxX = map.First().Length-1;
            var maxY = map.Count-1;

            return possiblePositions
                    .Where(pos => pos.X >= 0 && pos.X <= maxX)
                    .Where(pos => pos.Y >= 0 && pos.Y <= maxY)
                    .Where(pos => map[pos.Y][pos.X] == ' ' || map[pos.Y][pos.X] == 'B')
                    .ToList();
        }
    }

    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Steps { get; set; }
        public int Distance { get; set; }
        public int StepsDistance => Steps + Distance;
        public Position? Previous { get; set; }

        public void SetDistance(int targetX, int targetY)
        {
            Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }
    }
}