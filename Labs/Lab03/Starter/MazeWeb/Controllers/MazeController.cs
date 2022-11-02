﻿using Algorithms;
using MazeGrid;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using SixLabors.ImageSharp;
using MazeGeneration;

namespace maze_web.Controllers;

public class MazeController : Controller
{
    public IActionResult Index(int size, string algo, MazeColor color)
    {
        // return Content($"{size}, {algo}, {color}");

        int mazeSize = GetSize(size);
        IMazeAlgorithm algorithm = GetAlgorithm(algo);
        Image mazeImage = GenerateMazeImage(mazeSize, algorithm, color);
        byte[] byteArray = ConvertToByteArray(mazeImage);
        return File(byteArray, "image/png");
    }

    private int GetSize(int size)
    {
        int mazeSize = 15;
        if (size > 0)
        {
            mazeSize = size;
        }
        return mazeSize;
    }

    private IMazeAlgorithm GetAlgorithm(string algo)
    {
        IMazeAlgorithm? algorithm = new RecursiveBacktracker();
        if (!string.IsNullOrEmpty(algo))
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(RecursiveBacktracker));
            Type? algoType = assembly?.GetType($"Algorithms.{algo}", false, true);
            if (algoType != null)
            {
                algorithm = Activator.CreateInstance(algoType) as IMazeAlgorithm;
            }
        }
        return algorithm!;
    }

    private Image GenerateMazeImage(int mazeSize, IMazeAlgorithm algorithm, MazeColor color)
    {
        IMazeGenerator generator =
            new MazeGenerator(
                new ColorGrid(mazeSize, mazeSize, color),
                algorithm);

        Image maze = generator.GetGraphicalMaze(true);
        return maze;
    }

    private static byte[] ConvertToByteArray(Image img)
    {
        using var stream = new MemoryStream();
        img.SaveAsPng(stream);
        return stream.ToArray();
    }
}
