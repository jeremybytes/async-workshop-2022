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

    public void GenerateMaze()
    {
        algorithm.CreateMaze(mazeGrid);
        isGenerated = true;
    }

    public string GetTextMaze(bool includePath = false)
    {
        if (!isGenerated)
            GenerateMaze();

        if (includePath)
        {
            Cell start = mazeGrid.GetCell(mazeGrid.Rows / 2, mazeGrid.Columns / 2);
            mazeGrid.path = start.GetDistances().PathTo(mazeGrid.GetCell(mazeGrid.Rows - 1, 0));
        }

        var result = mazeGrid.ToString();
        return result;
    }

    public Image GetGraphicalMaze(bool includeHeatMap = false)
    {
        if (!isGenerated)
            GenerateMaze();

        if (includeHeatMap)
        {
            Cell start = mazeGrid.GetCell(mazeGrid.Rows / 2, mazeGrid.Columns / 2);
            mazeGrid.distances = start.GetDistances();
        }
        var result = mazeGrid.ToPng(30);
        return result;
    }
}
