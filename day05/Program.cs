using System.Diagnostics;
using System.Text.RegularExpressions;

internal partial class Program
{
    private static string Part1()
    {
        return Solve(CrateMover9000);
    }

    private static string Part2()
    {
        return Solve(CrateMover9001);
    }
    private static void CrateMover9000(int quantity, Stack<char> source, Stack<char> destination)
    {
        for (int i = 0; i < quantity; i++)
        {
            destination.Push(source.Pop());
        }
    }
    private static void CrateMover9001(int quantity, Stack<char> source, Stack<char> destination)
    {
        Stack<char> temp = new();
        for (int i = 0; i < quantity; i++)
        {
            temp.Push(source.Pop());
        }
        for (int i = 0; i < quantity; i++)
        {
            destination.Push(temp.Pop());
        }
    }

    [GeneratedRegex("move (?<quantity>\\d+) from (?<source>\\d) to (?<destination>\\d)")]
    private static partial Regex MoveRegex();
    private static string Solve(Action<int,Stack<char>, Stack<char>> cranemover)
    {
        Stack<char>[] stacks = Enumerable.Range(0, 9).Select(x => new Stack<char>()).ToArray();
        string[] lines = File.ReadAllLines("input.txt");
        int i = 0;
        while (true)
        {
            if (lines[i][1] == '1') // we do not have numbered crates
            {
                break;
            }
            for (int j = 0; j < stacks.Length; j++)
            {
                if (lines[i][j * 4 + 1] == ' ')
                {
                    continue;
                }
                stacks[j].Push(lines[i][j * 4 + 1]);
            }
            i++;
        }

        for (int j = 0; j < stacks.Length; j++)
        {
            // this looks like we are getting the same stack
            // but in fact we reverse it
            stacks[j] = new Stack<char>(stacks[j]);
        }

        for (i += 2; i < lines.Length; i++)
        {
            Match m = MoveRegex().Match(lines[i]);
            int quantity = int.Parse(m.Groups["quantity"].Value);
            int source = int.Parse(m.Groups["source"].Value);
            int destination = int.Parse(m.Groups["destination"].Value);
            cranemover(quantity, stacks[source - 1], stacks[destination - 1]);
        }

        return new string(stacks.Select(x => x.Peek()).ToArray());
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