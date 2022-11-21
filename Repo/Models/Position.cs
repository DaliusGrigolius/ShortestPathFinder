namespace GameLogic.Models
{
    public class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Steps { get; set; }
        public int Distance { get; set; }
        public int StepsDistance => Steps + Distance;
        public Position? Previous { get; set; }

        public Position()
        {

        }

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }
        
        public void SetDistance(int exitRow, int exitCol)
        {
            Distance = Math.Abs(exitRow - Row) + Math.Abs(exitCol - Col);
        }
    }
}
