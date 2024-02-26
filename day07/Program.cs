using System.Diagnostics;
using System.Text.RegularExpressions;

internal partial class Program
{
    private static string Part1()
    {
        return Solve().Item1.ToString();
    }
    private static string Part2()
    {
        return Solve().Item2.ToString();
    }

    class Node
    {
        public Node? Parent;
        public int TotalSize;
    }

    [GeneratedRegex("(?<size>\\d+) (?<name>.+)")]
    private static partial Regex FileRegex();
    [GeneratedRegex("\\$ cd (?<name>.+)")]
    private static partial Regex CdRegex();

    private static (int, int) Solve()
    {
        int part1Result = 0;
        Node root = new();
        Node current = root;
        SortedList<int, bool> sizesForPart2 = new();

        Node WalkUp(Node current)
        {
            if (current.Parent == null)
            {
                throw new ApplicationException("Parent is null");
            }

            var size = current.TotalSize;
            sizesForPart2.TryAdd(size, true);
            if (size <= 100000)
            {
                part1Result += size;
            }

            current = current.Parent;
            current.TotalSize += size;
            return current;
        }

        foreach (var line in File.ReadAllLines("input.txt"))
        {
            var cd = CdRegex().Match(line);
            if (cd.Success)
            {
                string name = cd.Groups["name"].Value;
                if (name == "/")
                {
                    continue;
                }
                // Walk Up
                if (name == "..")
                {
                    current = WalkUp(current);
                    continue;
                }
                // Walk down
                current = new Node() { Parent = current };
                continue;
            }
            var file = FileRegex().Match(line);
            if (file.Success)
            {
                current.TotalSize += int.Parse(file.Groups["size"].Value);
            }
        }

        // Walk beack up to root
        while (current.Parent != null)
        {
            current = WalkUp(current);
        }

        // Due to "unstated invariant" this is most likely is not required
        sizesForPart2.TryAdd(current.TotalSize, true);
        // Due to "unstated invariant" this is most likely is not required either
        if (current.TotalSize <= 100000)
        {
            part1Result += current.TotalSize;
        }

        int threshold = current.TotalSize + 30000000 - 70000000;

        return (part1Result, sizesForPart2.First(x => x.Key >= threshold).Key);

    }

    // From here common code for all days goes
    public static string FormatDuration(TimeSpan ts)
    {
        return ts.Days != 0 ? $"{ts.TotalDays} days"
                : ts.Hours != 0 ? $"{ts.TotalHours} hours"
                : ts.Minutes != 0 ? $"{ts.TotalMinutes} minutes"
                : ts.Seconds != 0 ? $"{ts.TotalSeconds} seconds"
                : $"{ts.TotalMilliseconds} milliseconds";
    }

    private static void DoAndReport(int day, int part, Func<string> func)
    {
        Stopwatch sw = Stopwatch.StartNew();
        string result = func();
        TimeSpan duration = sw.Elapsed;
        Console.WriteLine($"Day {day}, Part {part}, {result} ({FormatDuration(duration)})");
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex DayRegex();

    private static void Main()
    {
        int day = int.Parse(DayRegex().Match(typeof(Program).Assembly.ManifestModule.Name).Value);
        DoAndReport(day, 1, Part1);
        DoAndReport(day, 2, Part2);
    }

}