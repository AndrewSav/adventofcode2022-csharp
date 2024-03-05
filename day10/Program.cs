using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;


internal static class ResultMatrixExtensions
{
    public static void SetSprite(this bool[][] m, int x, int c)
    {
        int xx = (c - 1) % 40;
        if (Math.Abs(xx - x) > 1) return;
        int yy = (c - 1) / 40;
        m[yy][xx] = true;
    }
}

internal static class Font
{
    private const string FontSmallLetters = "ABCEFGHIJKLOPRSUYZ";
    private static readonly Dictionary<char,int> SmallLetterWidths  = new()
    {
        { 'A', 4 },
        { 'B', 4 },
        { 'C', 4 },
        { 'E', 4 },
        { 'F', 4 },
        { 'G', 4 },
        { 'H', 4 },
        { 'I', 3 },
        { 'J', 4 },
        { 'K', 4 },
        { 'L', 4 },
        { 'O', 4 },
        { 'P', 4 },
        { 'R', 4 },
        { 'S', 4 },
        { 'U', 4 },
        { 'Y', 5 },
        { 'Z', 4 }
    };

    private const string FontSmall = """
                                     .##..###...##..####.####..##..#..#.###...##.#..#.#.....##..###..###...###.#..#.#...#.####
                                     #..#.#..#.#..#.#....#....#..#.#..#..#.....#.#.#..#....#..#.#..#.#..#.#....#..#.#...#....#
                                     #..#.###..#....###..###..#....####..#.....#.##...#....#..#.#..#.#..#.#....#..#..#.#....#.
                                     ####.#..#.#....#....#....#.##.#..#..#.....#.#.#..#....#..#.###..###...##..#..#...#....#..
                                     #..#.#..#.#..#.#....#....#..#.#..#..#..#..#.#.#..#....#..#.#....#.#.....#.#..#...#...#...
                                     #..#.###...##..####.#.....###.#..#.###..##..#..#.####..##..#....#..#.###...##....#...####
                                     """;

    private const string FontLargeLetters = "ABCEFGHJKLNPRXZ";
    private const string FontLarge = """
                                     ..##...#####...####..######.######..####..#....#....###.#....#.#......#....#.#####..#####..#....#.######
                                     .#..#..#....#.#....#.#......#......#....#.#....#.....#..#...#..#......##...#.#....#.#....#.#....#......#
                                     #....#.#....#.#......#......#......#......#....#.....#..#..#...#......##...#.#....#.#....#..#..#.......#
                                     #....#.#....#.#......#......#......#......#....#.....#..#.#....#......#.#..#.#....#.#....#..#..#......#.
                                     #....#.#####..#......#####..#####..#......######.....#..##.....#......#.#..#.#####..#####....##......#..
                                     ######.#....#.#......#......#......#..###.#....#.....#..##.....#......#..#.#.#......#..#.....##.....#...
                                     #....#.#....#.#......#......#......#....#.#....#.....#..#.#....#......#..#.#.#......#...#...#..#...#....
                                     #....#.#....#.#......#......#......#....#.#....#.#...#..#..#...#......#...##.#......#...#...#..#..#.....
                                     #....#.#....#.#....#.#......#......#...##.#....#.#...#..#...#..#......#...##.#......#....#.#....#.#.....
                                     #....#.#####...####..######.#.......###.#.#....#..###...#....#.######.#....#.#......#....#.#....#.######
                                     """;

    public static string GetOCRKey(bool[][] plot, int x, int y, int width, int height)
    {
        StringBuilder sb = new();
        for (int dy = y; dy <y+height; dy++)
        {
            sb.Append(plot[dy][x..(x + width)].Select(z => z ? '#':'.').ToArray());
        }
        return sb.ToString();
    }

    private static Dictionary<string, char>? _smallFontMap;
    private static readonly object Lock = new();

    private static Dictionary<string, char> CreateFontMap(string font, string letters, Dictionary<char, int> widths, int height)
    {
        Dictionary<string, char> result = new();
        string[] lines = font.Split('\n');
        bool[][] plot = new bool[lines.Length][];
        for (int y = 0; y < plot.Length; y++)
        {
            plot[y] = lines[y].Trim('\r').ToCharArray().Select(z => z == '#' ? true: false).ToArray();
        }

        int offset = 0;
        foreach (char r in letters)
        {
            int width = widths[r];
            result[GetOCRKey(plot, offset, 0, width, height)] = r;
            offset += width + 1;
        }
        return result;
    }

    private static void EnsureMap()
    {
        lock (Lock)
        {
            _smallFontMap ??= CreateFontMap(FontSmall, FontSmallLetters, SmallLetterWidths, 6);
        }
    }

    public static char GetLetterByOCRKey(string ocrKey)
    {
        EnsureMap();
        return _smallFontMap![ocrKey];
    }
    public static string OCR2022Day10Part2(bool[][] plot)
    {
        StringBuilder sb = new StringBuilder();
        int width;
        for (int x = 0; x < plot[0].Length; x+=width)
        {
            char letter = GetLetterByOCRKey(GetOCRKey(plot, x, 0, 4, 6));
            sb.Append(letter);
            width = SmallLetterWidths[letter] + 1;
        }
        return sb.ToString();
    }
}

internal partial class Program
{
    private static string Part1()
    {
        string[] lines = File.ReadAllLines("input.txt");
        int x = 1;
        int c = 1;
        int nextCheckpoint = 20;
        int checkpointIncrement = 40;
        int lastCheckpoint = 220;

        int result = 0;

        foreach (string line in lines)
        {
            int previous = x;
            char instruction = line[0];
            if (instruction == 'a')
            {
                int value = int.Parse(line[5..]);
                c += 2;
                x += value;
            }
            else
            {
                c++;
            }

            if (c < nextCheckpoint) continue;
            result += nextCheckpoint * (c!= nextCheckpoint && instruction == 'a' ? previous : x);
            if (nextCheckpoint >= lastCheckpoint)
            {
                return result.ToString();
            }
            nextCheckpoint += checkpointIncrement;
        }

        throw new ApplicationException("have not reached last checkpoint");
    }


    private static T[][] InitResultMatrix<T>(int y, int x)
    {
        T[][] resultMatrix = new T[y][];
        for (int yy = 0; yy < y; yy++)
        {
            resultMatrix[yy] = new T[x];
        }
        return resultMatrix;
    }

    private static string Part2()
    {
        string[] lines = File.ReadAllLines("input.txt");
        int x = 1;
        int c = 1;
        bool[][] resultMatrix = InitResultMatrix<bool>(6, 40);

        foreach (string line in lines)
        {
            char instruction = line[0];
            if (instruction == 'a')
            {
                resultMatrix.SetSprite(x, c);
                resultMatrix.SetSprite(x, c+1);
                int value = int.Parse(line[5..]);
                c += 2;
                x += value;
            }
            else
            {
                resultMatrix.SetSprite(x, c);
                c++;
            }
        }

        return Font.OCR2022Day10Part2(resultMatrix);
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
