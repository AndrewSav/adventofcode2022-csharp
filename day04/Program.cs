using System.Diagnostics;
using System.Text.RegularExpressions;

internal partial class Program
{
    private static IEnumerable<int[]> GetPairs()
    {
        return File.ReadAllLines("input.txt").Select(x => x.Split(',', '-').Select(int.Parse).ToArray());
    }

    private static string Part1()
    {
        return GetPairs().Aggregate(0, (a, x) => x[0] >= x[2] && x[1] <= x[3] || x[2] >= x[0] && x[3] <= x[1] ? a+1 : a).ToString();
    }

    private static string Part2()
    {
        return GetPairs().Aggregate(0, (a, x) =>
                x[0] <= x[2] && x[2] <= x[1] ||
                x[0] <= x[3] && x[3] <= x[1] ||
                x[2] <= x[0] && x[0] <= x[3] ||
                x[3] <= x[1] && x[1] <= x[3] ? a+1 : a).ToString();
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