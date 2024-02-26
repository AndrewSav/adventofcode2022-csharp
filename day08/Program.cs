using System.Diagnostics;
using System.Text.RegularExpressions;

internal partial class Program
{
    public delegate IEnumerable<T> GetPartMetricFunc<out T>(int[] arg);
    public delegate T AggregateFunc<T>(T acc, T val);
    public delegate int AggregateFinalResultFunc<in T>(int acc, T val);

    private static string Part1()
    {
        return Solve(false, GetVisibility, (x, y) => x | y, (x, y) => y ? x + 1: x).ToString();
    }

    private static string Part2()
    {
        return Solve(1, GetScenicScore, (x, y) => x * y, Math.Max).ToString();
    }

    private static int Solve<T>(T startingDefault,
        GetPartMetricFunc<T> getPart,
        AggregateFunc<T> aggregatePart,
        AggregateFinalResultFunc<T> aggregateFinal)
    {
        int[][] input = File.ReadAllLines("input.txt")
            .Select(x => x.ToCharArray().Select(y => int.Parse(new ReadOnlySpan<char>(in y))).ToArray())
            .ToArray();
        T[][] resultMatrix = InitResultMatrix(input.Length, startingDefault);
        // Horizontal
        CalculateDimension(getPart, aggregatePart, input, resultMatrix);
        input = Transpose(input);
        resultMatrix = Transpose(resultMatrix);
        // Vertical
        CalculateDimension(getPart, aggregatePart, input, resultMatrix);
        return AggregateFinalResult(resultMatrix, aggregateFinal);
    }

    private static void CalculateDimension<T>(GetPartMetricFunc<T> getPart,
        AggregateFunc<T> aggregatePart,
        int[][] input,
        T[][] resultMatrix)
    {
        for (int y = 0; y < input.Length; y++)
        {
            // Looking from the left/top
            int x = 0;
            foreach (T val in getPart(input[y]))
            {
                resultMatrix[y][x] = aggregatePart(resultMatrix[y][x], val);
                x++;
            }

            // Looking from the right/bottom
            x = input.Length - 1;
            foreach (T val in getPart(input[y].Reverse().ToArray()))
            {
                resultMatrix[y][x] = aggregatePart(resultMatrix[y][x], val);
                x--;
            }
        }
    }

    private static T[][] Transpose<T>(T[][] arr)
    {
        int rowCount = arr.Length;
        T[][] result = (T[][])arr.Clone();
        for (int i = 1; i < rowCount; i++)
        {
            for (int j = 0; j < i; j++)
            {
                (result[i][j], result[j][i]) = (result[j][i], result[i][j]);
            }
        }

        return result;
    }

    private static int AggregateFinalResult<T>(T[][] resultMatrix, AggregateFinalResultFunc<T> aggregate)
    {
        int result = 0;
        foreach (var line in resultMatrix)
        {
            for (int x = 0; x < resultMatrix.Length; x++)
            {
                result = aggregate(result, line[x]);
            }
        }
        return result;
    }

    private static T[][] InitResultMatrix<T>(int dimension, T initValue)
    {
        T[][] resultMatrix = new T[dimension][];
        for (int i = 0; i < resultMatrix.Length; i++)
        {
            resultMatrix[i] = new T[dimension];
            for (int j = 0; j < resultMatrix.Length; j++)
            {
                resultMatrix[i][j] = initValue;
            }
        }
        return resultMatrix;
    }

    private static IEnumerable<bool> GetVisibility(int[] input)
    {
        int max = -1;
        foreach (int i in input)
        {
            if (i > max)
            {
                max = i;
                yield return true;
            }
            else
            {
                yield return false;
            }
            if (max == 9) yield break;
        }
    }
    private static IEnumerable<int> GetScenicScore(int[] input)
    {
        int[] cur = new int[10];
        foreach (int i in input)
        {
            int result = cur[i];
            for (int j = 0; j < 10; j++)
            {
                if (i < j)
                {
                    cur[j]++;
                }
                else
                {
                    cur[j] = 1;
                }
            }
            yield return result;
        }
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
