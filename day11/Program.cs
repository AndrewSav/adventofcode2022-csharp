using System.Diagnostics;
using System.Text.RegularExpressions;


internal class Monkey
{

}

internal partial class Program
{
    private static string Part1()
    {
        string[] lines = File.ReadAllLines("input.txt");
        for (int i = 0; i < lines.Length; i++)
        {
            Match m = Regex.Match(lines[i], @"Monkey (?<i>\d):");
            int.Parse(m.Groups["i"].Value);
            Console.WriteLine(lines[i]);
        }
        return "1";

    }

    private static string Part2()
    {
        return "2";
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
