using System.Diagnostics;
using System.Text.RegularExpressions;

internal partial class Program
{
    private static string Part1()
    {
        return File.ReadAllLines("input.txt").Aggregate(0, (acc, line) =>
            acc + line[..(line.Length / 2)].
            Intersect(line[(line.Length / 2)..]).
            Select(c => c <= 'Z' ? c - 'A' + 27 : c - 'a' + 1).
            First()).ToString();
    }

    private static string Part2()
    {
        return File.ReadAllLines("input.txt").Chunk(3).Aggregate(0, (acc, chunk) =>
            acc + chunk[0].Intersect(chunk[1]).Intersect(chunk[2]).
            Select(c => c <= 'Z' ? c - 'A' + 27 : c - 'a' + 1).
            First()).ToString();
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