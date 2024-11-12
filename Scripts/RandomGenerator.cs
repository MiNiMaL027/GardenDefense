using System;

public static class RandomGenerator
{
    public readonly static Random Random = new Random();

    public static bool ChanceCheck(float chance)
    {
        if (Random.Next(0, 100) >= chance)
        {
            return false;
        }

        return true;
    }

    public static int GetRandomNumberRange(int max)
    {
        return Random.Next(max);
    }
}

