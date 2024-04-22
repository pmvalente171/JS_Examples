using System.Collections.Generic;

namespace GameArchitecture.Util
{
    public static class EnumerableExtension
    {
        public static IEnumerable<double> CumulativeSum(this IEnumerable<double> sequence)
        {
            double sum = 0;
            foreach(var item in sequence)
            {
                sum += item;
                yield return sum;
            }        
        }
        
        public static IEnumerable<float> CumulativeSum(this IEnumerable<float> sequence)
        {
            float sum = 0;
            foreach(var item in sequence)
            {
                sum += item;
                yield return sum;
            }        
        }
    }
}