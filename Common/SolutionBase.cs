namespace AoC.Common;

public abstract class SolutionBase
{
    private readonly string _inputPath;
    private readonly string _logPrefix;

    protected SolutionBase()
    {
        SolutionTypes.ParseYearDay(GetType(), out var year, out var day);

        var formattedYear = year.ToString("D4");
        var formattedDay = day.ToString("D2");

        _inputPath = $"Y{formattedYear}/D{formattedDay}/input.txt";
        _logPrefix = $"[{formattedYear}-{formattedDay}] ";
    }

    public abstract void Run();

    protected string ReadInputAsText() => File.ReadAllText(_inputPath);
    protected string[] ReadInputAsLines() => File.ReadAllLines(_inputPath);

    public void Log<T>(in T value)
    {
        Console.WriteLine(_logPrefix + value);
    }
}