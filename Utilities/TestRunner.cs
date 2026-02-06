public static class TestRunner
{
    private static int _passed = 0;
    private static int _failed = 0;
    private static List<string> _failures = new();

    public static void Expect(bool condition, string testName)
    {
        if (condition)
        {
            _passed++;
            Console.WriteLine($"  ✓ {testName}");
        }
        else
        {
            _failed++;
            _failures.Add(testName);
            Console.WriteLine($"  ✗ {testName}");
        }
    }

    public static void ExpectEqual<T>(T expected, T actual, string testName)
    {
        var condition = EqualityComparer<T>.Default.Equals(expected, actual);
        if (condition)
        {
            _passed++;
            Console.WriteLine($"  ✓ {testName}");
        }
        else
        {
            _failed++;
            _failures.Add($"{testName} (expected: {expected}, got: {actual})");
            Console.WriteLine($"  ✗ {testName} (expected: {expected}, got: {actual})");
        }
    }

    public static void ExpectNotNull(object? value, string testName)
    {
        Expect(value != null, testName);
    }

    public static void ExpectNull(object? value, string testName)
    {
        Expect(value == null, testName);
    }

    public static void ExpectGreaterThan(int actual, int threshold, string testName)
    {
        Expect(actual > threshold, $"{testName} (got: {actual}, expected > {threshold})");
    }

    public static void Describe(string suiteName)
    {
        Console.WriteLine($"\n{suiteName}");
        Console.WriteLine(new string('-', suiteName.Length));
    }

    public static void Summary()
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine($"Results: {_passed} passed, {_failed} failed");
        Console.WriteLine("========================================");

        if (_failures.Any())
        {
            Console.WriteLine("\nFailed tests:");
            foreach (var failure in _failures)
            {
                Console.WriteLine($"  • {failure}");
            }
        }
    }

    public static void Reset()
    {
        _passed = 0;
        _failed = 0;
        _failures.Clear();
    }
}
