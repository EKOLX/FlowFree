using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileManager
{
    private readonly string level4Path = $"{Application.streamingAssetsPath}/Levels/4_1.json";

    public int[,] ReadLevel(int size)
    {
        var rows = File.ReadAllLines(level4Path); // TODO: Choose data path dynamic or preload them

        int[,] level2D = new int[size, size];

        for (int row = 0; row < rows.Count(); row++)
        {
            var columns = rows[row].Split(' ');
            for (int column = 0; column < columns.Count(); column++)
            {
                level2D[row, column] = Convert.ToInt16(columns[column]);
            }
        }

        return level2D;
    }
}
