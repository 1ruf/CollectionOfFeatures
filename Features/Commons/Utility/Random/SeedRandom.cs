public static class SeedRandom
{
    private static uint _seedState = 1;

    private const int Step1 = 13;
    private const int Step2 = 17;
    private const int Step3 = 5;

    public static void SetSeed(uint seed)
    {
        _seedState = seed == 0 ? 1u : seed;
    }

    private static uint Next()
    {
        uint x = _seedState;
        x ^= x << Step1;
        x ^= x >> Step2;
        x ^= x << Step3;
        _seedState = x;
        return x;
    }

    public static int Range(int min, int max)
    {
        if (min >= max)
            (min, max) = (max, min);

        return (int)(Next() % (uint)(max - min)) + min;
    }
}