using MazeGrid;

namespace Algorithms;

public interface IMazeAlgorithm
{
    void CreateMaze(Grid grid);
    Task CreateMazeAsync(Grid grid)
    {
        return Task.Run(() => CreateMaze(grid));
    }
}
