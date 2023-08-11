using System.Diagnostics;
using System.Text.RegularExpressions;

internal partial class Program
{
    private static string Solve(int length)
    {
        string data = File.ReadAllLines("input.txt")[0];
        Dictionary<char, int> marker = new();
        for (int i = 0; i < data.Length; i++)
        {
            if (i - length >= 0)
            {
                if (marker[data[i - length]] == 1)
                {
                    marker.Remove(data[i - length]);
                }
                else
                {
                    marker[data[i - length]]--;
                }
            }
            if (marker.ContainsKey(data[i]))
            {
                marker[data[i]]++;
            }
            else
            {
                marker[data[i]] = 1;
            }
            if (marker.Count == length)
            {
                return (i + 1).ToString();
            }
        }
        throw new ApplicationException("no result");
    }

    private static string Part1()
    {
        return Solve(4);
    }

    private static string Part2()
    {
        return Solve(14);
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