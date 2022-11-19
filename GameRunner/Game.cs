using GameLogic;

namespace GameRunner;

public class Game : IGame
{
    public int Run(string filePath)
    {
        IGameState gameState = new GameState(filePath);

        return gameState.GetShortestPath();
    }
}