using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Dots
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal outY { get; set; }
        public decimal[] DistanceToCentersByX { get; set; }
        public decimal[] DistanceToCentersByXY { get; set; }

        public decimal[] BelongingCluster { get; set; }
        //public decimal[] BelongingClusterResult { get; set; }



    }
}
