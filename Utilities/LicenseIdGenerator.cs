public static class LicenseIdGenerator
{
    private static readonly Random _random = new();

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
