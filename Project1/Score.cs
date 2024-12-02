using System.Collections.Generic;
using System.Linq;

namespace Project1
{
    public class Score
    {
        // Method to calculate the average score from a list of scores
        public static double CalculateAverageScore(List<int> scores)
        {
            if (scores == null || scores.Count == 0)
                return 0;

            return scores.Average();
        }
    }
}
