using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

internal class Rope(int length)
{
    private readonly Point[] _knots = new Point[length];

    public void Up()
    {
        _knots[0].Y++;
        Normalise();
    }
    public void Down()
    {
        _knots[0].Y--;
        Normalise();
    }

    public void Left()
    {
        _knots[0].X--;
        Normalise();
    }

    public void Right()
    {
        _knots[0].X++;
        Normalise();
    }

    private void Normalise()
    {
        for (int i = 1; i < _knots.Length; i++)
        {
            var dx = _knots[i - 1].X - _knots[i].X;
            var dy = _knots[i - 1].Y - _knots[i].Y;

            if (Math.Abs(dx) > 1 || Math.Abs(dx) > 0 && Math.Abs(dy) > 1)
            {
                _knots[i].X += Math.Sign(dx);
            }
            if (Math.Abs(dy) > 1 || Math.Abs(dy) > 0 && Math.Abs(dx) > 1)
            {
                _knots[i].Y += Math.Sign(dy);
            }
        }
    }

    public Point Tail => _knots[^1];
}

internal partial class Program
{
    private static string Part1()
    {
        return Solve(2).ToString();
    }
    
    private static string Part2()
    {
        return Solve(10).ToString();
    }

    private static int Solve(int length)
    {
        string[] lines = File.ReadAllLines("input.txt");
        Rope r = new(length);
        HashSet<Point> hs = [];
        foreach (string line in lines)
        {
            char direction = line[0];
            int distance = int.Parse(line[2..]);
            for (int i = 0; i < distance; i++)
            {
                switch (direction)
                {
                    case 'R':
                        r.Right();
                        break;
                    case 'L':
                        r.Left();
                        break;
                    case 'U':
                        r.Up();
                        break;
                    case 'D':
                        r.Down();
                        break;
                }

                hs.Add(r.Tail);
            }
        }

        return hs.Count;
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
