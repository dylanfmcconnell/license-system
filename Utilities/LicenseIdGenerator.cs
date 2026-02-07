/// <summary>
/// Generates unique license identifiers in the format of two uppercase letters followed by seven digits.
/// </summary>
public static class LicenseIdGenerator
{
    private static readonly Random _random = new();

    /// <summary>
    /// Generates a new unique license identifier (e.g., "AB1234567").
    /// </summary>
    /// <returns>A string containing two random uppercase letters followed by seven random digits.</returns>
    public static string GenerateId()
    {
        var letters = new char[2];
        for (int i = 0; i < 2; i++)
        {
            letters[i] = (char)_random.Next('A', 'Z' + 1);
        }

        var numbers = _random.Next(1000000, 10000000); // 7 digits

        return $"{new string(letters)}{numbers}";
    }
}
