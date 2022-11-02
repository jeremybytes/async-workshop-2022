using Algorithms;
using MazeGrid;
using SixLabors.ImageSharp;

namespace MazeGeneration;

public class MazeGenerator : IMazeGenerator
{
    private readonly Grid mazeGrid;
    private readonly IMazeAlgorithm algorithm;
    private bool isGenerated = false;

    public MazeGenerator(Grid grid, IMazeAlgorithm algorithm)
    {
        this.mazeGrid = grid;
        this.algorithm = algorithm;
    }

    public async Task GenerateMaze()
    {
        await algorithm.CreateMazeAsync(mazeGrid);
        isGenerated = true;
    }

    public async Task<string> GetTextMaze(bool includePath = false)
    {
        if (!isGenerated)
            await GenerateMaze();

        if (includePath)
        {
            Cell start = mazeGrid.GetCell(mazeGrid.Rows / 2, mazeGrid.Columns / 2);
            mazeGrid.path = start.GetDistances().PathTo(mazeGrid.GetCell(mazeGrid.Rows - 1, 0));
        }

        var result = await mazeGrid.ToStringAsync();
        return result;
    }

    public async Task<Image> GetGraphicalMaze(bool includeHeatMap = false)
    {
        if (!isGenerated)
            await GenerateMaze();

        if (includeHeatMap)
        {
            Cell start = mazeGrid.GetCell(mazeGrid.Rows / 2, mazeGrid.Columns / 2);
            mazeGrid.distances = start.GetDistances();
        }
        var result = await mazeGrid.ToPngAsync(30);
        return result;
    }
}
