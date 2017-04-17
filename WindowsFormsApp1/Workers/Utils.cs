using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class Utils
    {
        public static decimal SimpleDistance(decimal X, decimal Y)
        {
            return Math.Abs(X - Y);
        }

        public static decimal Distance(Dots tuple, decimal ClusterCenterC, decimal ClusterCenterY)
        {
            // Euclidean distance between two vectors
            decimal sumSquaredDiffs = 0.0M;
            sumSquaredDiffs += Convert.ToDecimal(Math.Pow((Convert.ToDouble(tuple.X - ClusterCenterC)), 2));
            sumSquaredDiffs += Convert.ToDecimal(Math.Pow((Convert.ToDouble(tuple.Y - ClusterCenterY)), 2));

            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(sumSquaredDiffs)));
        }
    }
}
