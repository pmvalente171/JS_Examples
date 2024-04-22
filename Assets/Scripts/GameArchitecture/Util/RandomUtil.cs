using System;

namespace GameArchitecture.Util
{
    public class RandomUtil
    {
        public static int RandomInt(int min, int max) => 
            UnityEngine.Random.Range(min, max);
        
        public static float RandomFloat(float min, float max) => 
            UnityEngine.Random.Range(min, max);
        
        public static bool RandomBool() => 
            UnityEngine.Random.Range(0, 2) == 0;
        
        public static T RandomEnum<T>() where T : Enum => 
            (T) Enum.GetValues(typeof(T)).GetValue(RandomInt(0, Enum.GetValues(typeof(T)).Length));
        
        public static T RandomEnum<T>(T[] values) where T : Enum => 
            values[RandomInt(0, values.Length)];
        
        public static T RandomEnum<T>(T[] values, int[] weights) where T : Enum
        {
            int totalWeight = 0;
            foreach (int weight in weights)
                totalWeight += weight;
            
            int randomWeight = RandomInt(0, totalWeight);
            int weightSum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                weightSum += weights[i];
                if (randomWeight < weightSum)
                    return values[i];
            }
            return values[0];
        }
        
        public static T RandomEnum<T>(T[] values, float[] weights) where T : Enum
        {
            float totalWeight = 0;
            foreach (float weight in weights)
                totalWeight += weight;
            
            float randomWeight = RandomFloat(0, totalWeight);
            float weightSum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                weightSum += weights[i];
                if (randomWeight < weightSum)
                    return values[i];
            }
            return values[0];
        }
        
        public static T RandomEnum<T>(T[] values, double[] weights) where T : Enum
        {
            double totalWeight = 0;
            foreach (double weight in weights)
                totalWeight += weight;
            
            double randomWeight = RandomDouble(0, totalWeight);
            double weightSum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                weightSum += weights[i];
                if (randomWeight < weightSum)
                    return values[i];
            }
            return values[0];
        }
        
        public static double RandomDouble(double min, double max) => 
            UnityEngine.Random.Range((float) min, (float) max);
        
        public static double RandomNormalDistribution(double mean, double stdDev)
        {
            double u1 = 1.0 - RandomDouble(0, 1);
            double u2 = 1.0 - RandomDouble(0, 1);
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }
    }
}