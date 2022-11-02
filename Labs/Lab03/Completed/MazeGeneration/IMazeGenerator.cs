using SixLabors.ImageSharp;

namespace MazeGeneration;

public interface IMazeGenerator
{
    Task GenerateMaze();
    Task<Image> GetGraphicalMaze(bool includeHeatMap = false);
    Task<string> GetTextMaze(bool includePath = false);
}