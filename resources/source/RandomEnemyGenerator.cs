using Godot;
using System;

public static class RandomEnemyGenerator
{
    // T1 enemy probability will not go below this vallue * 100
    private static float _t1MinProbability = 0.1f;
    // T1 probability will stop decreasing at escalation = this value * 100
    private static float _t1WeightCap = 0.8f;

    private static float _t2ProbabilityIncreaseRate = 0.6f;
    private static float _t2ProbabilityMin = 0f;
    private static float _t2ProbailityMax = 1f;

    private static float _t3ProbabilityThreshold = 0.6f;
    private static float _t3ProbabilityMin = 0.05f;
    private static float _t3ProbabilityMax = 0.2f;


    public static int GetMinEnemyCount(int escalation)
    {
        Random rnd = new Random();
        int minEnemyCount = 1;

        if(escalation < 40)
        {
            minEnemyCount = 1 + rnd.Next(0, 4);
        }
        else if(escalation < 60)
        {
            minEnemyCount = 2 + rnd.Next(0, 2);
        }
        else
        {
            minEnemyCount = 4;
        }

        return minEnemyCount;
    }

    public static int GetRandomEnemyTier(int escalation)
    {
        Random rnd = new Random();
        //PLACEHOLDER
        float escalationNormal = escalation / 100f;

        float t1Weight = Mathf.Lerp(1.0f, _t1MinProbability, escalationNormal/_t1WeightCap);
        float t2Weight = Mathf.Clamp(escalationNormal * _t2ProbabilityIncreaseRate, _t2ProbabilityMin, _t2ProbailityMax);
        float t3Weight = 0;
        if (escalationNormal >= _t3ProbabilityThreshold)
        {
            t3Weight = Mathf.Lerp(_t3ProbabilityMin, _t3ProbabilityMax, escalationNormal);
        }

        float total = t1Weight + t2Weight + t3Weight;
        float roll = (float)rnd.NextDouble() * total;

        if (roll < t1Weight)
        {
            return 1;
        }
        else if (roll < t1Weight + t2Weight)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
}
