using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class CityReader
{
    public class ParsedData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public TileTypes[] Types;
    }

    private const String FileName = "City.txt";

    private ParsedData ParseHeader(string[] iterator, out int lineEnd)
    {
        ParsedData parsed = new ParsedData();
        lineEnd = 0;

        foreach (var line in iterator)
        {
            lineEnd++;
            if (line.StartsWith("//"))
            {
                continue;
            }

            var data = line.Split(" ");
            if (data.Length != 4)
            {
                return null;
            }

            if (data[0] != "width" || data[2] != "height")
            {
                return null;
            }

            if (int.TryParse(data[1], out var result))
            {
                parsed.Width = result;
            }
            else
            {
                return null;
            }

            if (int.TryParse(data[3], out result))
            {
                parsed.Height = result;
                break;
            }
            else
            {
                return null;
            }
        }

        return parsed;
    }

    public ParsedData ReadCity()
    {
        var iterator = File.ReadAllLines(Application.dataPath + "/" + FileName);

        var parsed = ParseHeader(iterator, out int start);
        
        if (parsed is null)
        {
            return null;
        }

        var count = 0;

        parsed.Types = new TileTypes[parsed.Width * parsed.Height];

        foreach (var line in iterator.Skip(start))
        {
            if (line.StartsWith("//"))
            {
                continue;
            }

            if (count == parsed.Height)
            {
                return null;
            }

            var split = line.Split(" ");
            if (split.Length != parsed.Width)
            {
                return null;
            }

            var index = 0;

            foreach (var num in split)
            {
                if (TileTypes.TryParse(num, out TileTypes result))
                {
                    parsed.Types[index + (parsed.Height - count - 1) * parsed.Height] = result;
                    index++;
                }
                else
                {
                    return null;
                }
            }

            count++;
        }

        return parsed;
    }
}