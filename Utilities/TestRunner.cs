/// <summary>
/// A lightweight test runner utility that tracks test assertions and provides summary reporting.
/// </summary>
public static class TestRunner
{
    private static int _passed = 0;
    private static int _failed = 0;
    private static List<string> _failures = new();

    /// <summary>
    /// Asserts that a condition is true and records the result.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="testName">The descriptive name of the test.</param>
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

    /// <summary>
    /// Asserts that two values are equal using the default equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="testName">The descriptive name of the test.</param>
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

    /// <summary>
    /// Asserts that a value is not null.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="testName">The descriptive name of the test.</param>
    public static void ExpectNotNull(object? value, string testName)
    {
        Expect(value != null, testName);
    }

    /// <summary>
    /// Asserts that a value is null.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="testName">The descriptive name of the test.</param>
    public static void ExpectNull(object? value, string testName)
    {
        Expect(value == null, testName);
    }

    /// <summary>
    /// Asserts that an integer value is greater than a specified threshold.
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="threshold">The threshold the value must exceed.</param>
    /// <param name="testName">The descriptive name of the test.</param>
    public static void ExpectGreaterThan(int actual, int threshold, string testName)
    {
        Expect(actual > threshold, $"{testName} (got: {actual}, expected > {threshold})");
    }

    /// <summary>
    /// Prints a test suite header to the console.
    /// </summary>
    /// <param name="suiteName">The name of the test suite.</param>
    public static void Describe(string suiteName)
    {
        Console.WriteLine($"\n{suiteName}");
        Console.WriteLine(new string('-', suiteName.Length));
    }

    /// <summary>
    /// Prints a summary of all test results, including pass/fail counts and a list of failures.
    /// </summary>
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

    /// <summary>
    /// Resets all test counters and clears the failure list.
    /// </summary>
    public static void Reset()
    {
        _passed = 0;
        _failed = 0;
        _failures.Clear();
    }
}
