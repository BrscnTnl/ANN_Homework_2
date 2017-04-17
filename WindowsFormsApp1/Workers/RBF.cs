using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class RBF
    {
        public decimal[,] ClusterCenters;
        public decimal[] ClusterSpreads;
        public int numClusters;

        public decimal trainingCoefficient = 0.1m;
        public decimal Momentum = 0.99m;

        private decimal Bias = 1;
        public int iterationLimit = 100;
        public decimal[] Weights;

        public RBF(int ParamNumClusters, int numOfIterations)
        {
            iterationLimit = numOfIterations;
            numClusters = ParamNumClusters;
            AssignClusterCenters(numClusters);
            InitialCluster();
            FindSpreads();
            SimplePerceptronTrain();
            SimplePerceptronValidate();
            List<Dots> asdasd = ExtractData.GetDataTrain();
        }


        private void SimplePerceptronTrain()
        {
            InitializeWeights();
            decimal tempSum = 0;

            for (int i = 0; i < iterationLimit; i++)
            {
                foreach (Dots member in ExtractData.GetDataTrain())
                {

                    #region Compute Output
                    for (int j = 0; j < numClusters; j++)
                    {
                        decimal a = Activation(member.X, ClusterCenters[j, 0], ClusterSpreads[j]);
                        decimal w = Weights[j];
                        tempSum +=  a*w ;
                    }
                    #endregion

                    #region UpdateWeights
                    decimal delta = 0;
                    for (int j = 0; j < numClusters ; j++)
                    {
                        delta = trainingCoefficient * (member.Y - tempSum) * Activation(member.X, ClusterCenters[j, 0], ClusterSpreads[j]);
                        Weights[j] += delta;
                    }
                    

                    #endregion

                    tempSum = 0;
                }        


            }
        }

        public decimal MeanError = 0;
        public decimal SumOfSquareError = 0;
        public decimal SumOfSquareErrorMean = 0;
        public decimal SumOfError = 0;
        public decimal ErrorStdDev = 0;

        public decimal MeanValue = 0;
        public decimal SumOfSquareValue = 0;
        public decimal SumOfSquareValueMean = 0;
        public decimal SumOfValue = 0;
        public decimal ValueStdDev = 0;

        public decimal MeanOutput = 0;
        public decimal SumOfSquareOutput = 0;
        public decimal SumOfSquareOutputMean = 0;
        public decimal SumOfOutput = 0;
        public decimal OutputStdDev = 0;

        public int CorrectCount = 0;
        public int FalseCount = 0;
        private void SimplePerceptronValidate()
        {
            decimal LocalError = 0;


            CorrectCount = 0;
            FalseCount = 0;

            foreach (Dots member in ExtractData.GetDataValidation())
            {
                

                #region Compute Output

                decimal output = 0;
                for (int j = 0; j < numClusters ; j++)
                {
                    output += Activation(member.X, ClusterCenters[j, 0], ClusterSpreads[j]) * Weights[j];
                }

                member.outY = output;

                
                

                #endregion

                #region Information Collection

                LocalError = member.Y - output;

                if (Math.Abs(Convert.ToDouble(output)) <= Math.Abs(Convert.ToDouble(member.Y)) * 1.2 && Math.Abs(Convert.ToDouble(output)) >= Math.Abs(Convert.ToDouble(member.Y)) * 0.8)
                    CorrectCount++;
                else
                    FalseCount++;

                SumOfError += LocalError;
                SumOfSquareError += LocalError * LocalError;

                SumOfOutput += output;
                SumOfSquareOutput += output * output;

                SumOfValue += member.Y;
                SumOfSquareValue += member.Y * member.Y;

                #endregion
            }


            SumOfSquareErrorMean = SumOfSquareError / ExtractData.GetDataValidation().Count;
            MeanError = SumOfError / ExtractData.GetDataValidation().Count;
            ErrorStdDev = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((SumOfSquareError - (MeanError * MeanError * ExtractData.GetDataValidation().Count)) / ExtractData.GetDataValidation().Count)));

            SumOfSquareValueMean = SumOfSquareValue / ExtractData.GetDataValidation().Count;
            MeanValue = SumOfValue / ExtractData.GetDataValidation().Count;
            ValueStdDev = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((SumOfSquareValue - (MeanValue * MeanValue * ExtractData.GetDataValidation().Count)) / ExtractData.GetDataValidation().Count)));

            SumOfSquareOutputMean = SumOfSquareOutput / ExtractData.GetDataValidation().Count;
            MeanOutput = SumOfOutput / ExtractData.GetDataValidation().Count;
            OutputStdDev = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((SumOfSquareOutput - (MeanOutput * MeanOutput * ExtractData.GetDataValidation().Count)) / ExtractData.GetDataValidation().Count)));

        }



        private void InitializeWeights() // helper for ctor
        {
            Random rnd = new Random();
            Weights = new decimal[numClusters];

            for (int i = 0; i < numClusters; i++)
            {
                Weights[i] = (0.01m - 0.001m) * Convert.ToDecimal(rnd.NextDouble()) + 0.001m;
            }
        }


        public void AssignClusterCenters(int numClusters)
        {
            //double MinX = -2.9998796;
            //double MaxX = 2.918494;
            //double MinY = -19.22561;
            //double MaxY = 30.430512;
            int MinX = -2999879;
            int MaxX = 2918494;
            int MinY = -19225610;
            int MaxY = 30430512;
            decimal dividor = 1000000m;
            ClusterCenters = new decimal[numClusters,2];
            Random rnd = new Random();
            // initialize weights and biases to small random values
            
            for (int i = 0; i < numClusters; i++)
            {
                ClusterCenters[i, 0] = (rnd.Next(MinX, MaxX)) / dividor; //X
                ClusterCenters[i,1] = (rnd.Next(MinY, MaxY)) / dividor; //Y
            }
        }


        private decimal Activation(decimal X, decimal Center, decimal spread)
        {
            return Convert.ToDecimal(Math.Exp(-1 * Math.Abs(Convert.ToDouble(X -Center)) / Convert.ToDouble(2 * spread * spread)));
        }

        private void InitialCluster()
        {
            foreach (Dots item in ExtractData.GetDataTrain())
            {
                decimal Dist = 500000m;
                for (int i = 0; i < numClusters; i++)
                {
                    item.DistanceToCentersByX[i] = Utils.SimpleDistance(ClusterCenters[i, 0], item.X);
                }
                
                for (int i = 0; i < numClusters; i++)
                {
                    if (item.DistanceToCentersByX[i] < Dist)
                    {
                        Dist = item.DistanceToCentersByX[i];
                        item.BelongingCluster[numClusters-1] = i+1;
                    }
                }
                
            }

            ReCalculateCenters();


            foreach (Dots item in ExtractData.GetDataValidation())
            {
                decimal Dist = 500000m;
                for (int i = 0; i < numClusters; i++)
                {
                    item.DistanceToCentersByX[i] = Utils.SimpleDistance(ClusterCenters[i, 0], item.X);
                }

                for (int i = 0; i < numClusters; i++)
                {
                    if (item.DistanceToCentersByX[i] < Dist)
                    {
                        Dist = item.DistanceToCentersByX[i];
                        item.BelongingCluster[numClusters - 1] = i + 1;
                    }
                }

            }

        }

        private void FindSpreads()
        {
            ClusterSpreads = new decimal[numClusters];

            for (int i = 0; i < numClusters; i++)
            {
                ClusterSpreads[i] = Utils.SimpleDistance(ClusterCenters[i, 0], ExtractData.GetDataTrain().Max(x=> x.X)) / 2;
            }
        }

        private void ReCalculateCenters()
        {
            for (int i = 0; i < numClusters; i++)
            {
                decimal count = Convert.ToDecimal(ExtractData.GetDataTrain().Where(x => x.BelongingCluster[numClusters - 1] == i + 1).Count());
                decimal summ = ExtractData.GetDataTrain().Where(x => x.BelongingCluster[numClusters - 1] == i + 1).Sum(y => y.X);
                if (count != 0)
                {
                    ClusterCenters[i, 0] = summ / count;
                }

            }


        }


        public override string ToString()
        {
            StringBuilder str = new StringBuilder();


            str.AppendLine(String.Format("RBF with {0} Epoch, {1} Training Coefficient, {2} Clusters", iterationLimit, trainingCoefficient, numClusters));


            //str.AppendLine("");
            //str.AppendLine("MeanError : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", MeanError) + " ");
            //str.AppendLine("SumOfSquareErrorMean : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfSquareErrorMean) + " ");
            //str.AppendLine("SumOfSquareError : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfSquareError) + " ");
            //str.AppendLine("SumOfError : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfError) + " ");
            //str.AppendLine("ErrorStdDev : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ErrorStdDev) + " ");
            //str.AppendLine("");
            //str.AppendLine("MeanValue : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", MeanValue) + " ");
            //str.AppendLine("SumOfSquareValue : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfSquareValue) + " ");
            //str.AppendLine("SumOfSquareValueMean : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfSquareValueMean) + " ");
            //str.AppendLine("SumOfValue : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfValue) + " ");
            //str.AppendLine("ValueStdDev : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ValueStdDev) + " ");
            //str.AppendLine("");
            //str.AppendLine("MeanOutput : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", MeanOutput) + " ");
            //str.AppendLine("SumOfSquareOutput : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfSquareOutput) + " ");
            //str.AppendLine("SumOfSquareOutputMean : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfSquareOutputMean) + " ");
            //str.AppendLine("SumOfOutput : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", SumOfOutput) + " ");
            //str.AppendLine("OutputStdDev : \t" + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", OutputStdDev) + " ");
            //str.AppendLine("");
            //str.AppendLine("Weights : ");
            //for (int j = 0; j < numClusters; j++)
            //{
            //    str.AppendLine(String.Format("Weight{0} : \t{1}", j + 1, String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Weights[j])));
            //}


            str.AppendLine("");
            str.AppendLine("MeanError : \t" + MeanError.ToString("#.##") + " ");
            str.AppendLine("SumOfSquareErrorMean : \t" + SumOfSquareErrorMean.ToString("#.##") + " ");
            str.AppendLine("SumOfSquareError : \t" + SumOfSquareError.ToString("#.##") + " ");
            str.AppendLine("SumOfError : \t" + SumOfError.ToString("#.##") + " ");
            str.AppendLine("ErrorStdDev : \t" + ErrorStdDev.ToString("#.##") + " ");
            str.AppendLine("");
            str.AppendLine("MeanValue : \t" + MeanValue.ToString("#.##") + " ");
            str.AppendLine("SumOfSquareValue : \t" + SumOfSquareValue.ToString("#.##") + " ");
            str.AppendLine("SumOfSquareValueMean : \t" + SumOfSquareValueMean.ToString("#.##") + " ");
            str.AppendLine("SumOfValue : \t" + SumOfValue.ToString("#.##") + " ");
            str.AppendLine("ValueStdDev : \t" + ValueStdDev.ToString("#.##") + " ");
            str.AppendLine("");
            str.AppendLine("MeanOutput : \t" + MeanOutput.ToString("#.##") + " ");
            str.AppendLine("SumOfSquareOutput : \t" + SumOfSquareOutput.ToString("#.##") + " ");
            str.AppendLine("SumOfSquareOutputMean : \t" + SumOfSquareOutputMean.ToString("#.##") + " ");
            str.AppendLine("SumOfOutput : \t" + SumOfOutput.ToString("#.##") + " ");
            str.AppendLine("OutputStdDev : \t" + OutputStdDev.ToString("#.##") + " ");
            str.AppendLine("");
            str.AppendLine("Weights : ");
            for (int j = 0; j < numClusters; j++)
            {
                str.AppendLine(String.Format("Weight{0} : \t{1}", j + 1, Weights[j].ToString("#.##")));
            }
            str.AppendLine("");
            str.AppendLine("");


            


            return str.ToString();
        }

        //public List<Dots> ComputeClusters()
        //{
        //    double UF_MeanError = 0;
        //    double NF_MeanError = 0;

        //    double UF_Spread_1 = 0;
        //    double UF_Spread_2 = 0;
        //    double NF_Spread_1 = 0;
        //    double NF_Spread_2 = 0;
        //    double NF_Spread_3 = 0;
        //    double NF_Spread_4 = 0;
        //    double NF_Spread_5 = 0;

        //    List<Dots> TrainData = ExtractData.GetDataTrain();

        //    for (int j = 0; j < TrainData.Count; j++)
        //    {
        //        TrainData.ElementAt(j).UF_DistanceTo1 = Math.Sqrt(Math.Pow(TrainData.ElementAt(j).X - ExtractData.GetUnderFittingCenters()[0, 0], 2) + Math.Pow(TrainData.ElementAt(j).Y - ExtractData.GetUnderFittingCenters()[0, 1], 2));
        //        TrainData.ElementAt(j).UF_DistanceTo2 = Math.Sqrt(Math.Pow(TrainData.ElementAt(j).X - ExtractData.GetUnderFittingCenters()[1, 0], 2) + Math.Pow(TrainData.ElementAt(j).Y - ExtractData.GetUnderFittingCenters()[1, 1], 2));

        //        TrainData.ElementAt(j).NF_DistanceTo1 = Math.Sqrt(Math.Pow(TrainData.ElementAt(j).X - ExtractData.GetNormalFittingCenters()[0, 0], 2) + Math.Pow(TrainData.ElementAt(j).Y - ExtractData.GetNormalFittingCenters()[0, 1], 2));
        //        TrainData.ElementAt(j).NF_DistanceTo2 = Math.Sqrt(Math.Pow(TrainData.ElementAt(j).X - ExtractData.GetNormalFittingCenters()[1, 0], 2) + Math.Pow(TrainData.ElementAt(j).Y - ExtractData.GetNormalFittingCenters()[1, 1], 2));
        //        TrainData.ElementAt(j).NF_DistanceTo3 = Math.Sqrt(Math.Pow(TrainData.ElementAt(j).X - ExtractData.GetNormalFittingCenters()[2, 0], 2) + Math.Pow(TrainData.ElementAt(j).Y - ExtractData.GetNormalFittingCenters()[2, 1], 2));
        //        TrainData.ElementAt(j).NF_DistanceTo4 = Math.Sqrt(Math.Pow(TrainData.ElementAt(j).X - ExtractData.GetNormalFittingCenters()[3, 0], 2) + Math.Pow(TrainData.ElementAt(j).Y - ExtractData.GetNormalFittingCenters()[3, 1], 2));
        //        TrainData.ElementAt(j).NF_DistanceTo5 = Math.Sqrt(Math.Pow(TrainData.ElementAt(j).X - ExtractData.GetNormalFittingCenters()[4, 0], 2) + Math.Pow(TrainData.ElementAt(j).Y - ExtractData.GetNormalFittingCenters()[4, 1], 2));
        //    }

        //    for (int j = 0; j < TrainData.Count; j++)
        //    {
        //        if (TrainData.ElementAt(j).UF_DistanceTo1 < TrainData.ElementAt(j).UF_DistanceTo2)
        //        {
        //            TrainData.ElementAt(j).UF_CLUSTER = 1;
        //            UF_MeanError += TrainData.ElementAt(j).UF_DistanceTo1;
        //        }
        //        else
        //        {
        //            TrainData.ElementAt(j).UF_CLUSTER = 2;
        //            UF_MeanError += TrainData.ElementAt(j).UF_DistanceTo2;
        //        }

        //        if (TrainData.ElementAt(j).NF_DistanceTo1 < TrainData.ElementAt(j).NF_DistanceTo2 &&
        //            TrainData.ElementAt(j).NF_DistanceTo1 < TrainData.ElementAt(j).NF_DistanceTo3 &&
        //            TrainData.ElementAt(j).NF_DistanceTo1 < TrainData.ElementAt(j).NF_DistanceTo4 &&
        //            TrainData.ElementAt(j).NF_DistanceTo1 < TrainData.ElementAt(j).NF_DistanceTo5)
        //        {
        //            TrainData.ElementAt(j).NF_CLUSTER = 1;
        //            NF_MeanError += TrainData.ElementAt(j).NF_DistanceTo1;
        //        }
        //        else if (TrainData.ElementAt(j).NF_DistanceTo2 < TrainData.ElementAt(j).NF_DistanceTo3 &&
        //                 TrainData.ElementAt(j).NF_DistanceTo2 < TrainData.ElementAt(j).NF_DistanceTo4 &&
        //                 TrainData.ElementAt(j).NF_DistanceTo2 < TrainData.ElementAt(j).NF_DistanceTo5)
        //        {
        //            TrainData.ElementAt(j).NF_CLUSTER = 2;
        //            NF_MeanError += TrainData.ElementAt(j).NF_DistanceTo2;
        //        }
        //        else if (TrainData.ElementAt(j).NF_DistanceTo3 < TrainData.ElementAt(j).NF_DistanceTo4 &&
        //                 TrainData.ElementAt(j).NF_DistanceTo3 < TrainData.ElementAt(j).NF_DistanceTo5)
        //        {
        //            TrainData.ElementAt(j).NF_CLUSTER = 3;
        //            NF_MeanError += TrainData.ElementAt(j).NF_DistanceTo3;
        //        }
        //        else if (TrainData.ElementAt(j).NF_DistanceTo4 < TrainData.ElementAt(j).NF_DistanceTo5)
        //        {
        //            TrainData.ElementAt(j).NF_CLUSTER = 4;
        //            NF_MeanError += TrainData.ElementAt(j).NF_DistanceTo4;
        //        }
        //        else
        //        {
        //            TrainData.ElementAt(j).NF_CLUSTER = 5;
        //            NF_MeanError += TrainData.ElementAt(j).NF_DistanceTo5;
        //        }
        //    }



        //    NF_MeanError /= TrainData.Count;
        //    UF_MeanError /= TrainData.Count;

        //    NF_Spread_1 = TrainData.Where(y => y.NF_CLUSTER == 1).Max(x => x.NF_DistanceTo1);
        //    NF_Spread_2 = TrainData.Where(y => y.NF_CLUSTER == 2).Max(x => x.NF_DistanceTo2);
        //    NF_Spread_3 = TrainData.Where(y => y.NF_CLUSTER == 3).Max(x => x.NF_DistanceTo3);
        //    NF_Spread_4 = TrainData.Where(y => y.NF_CLUSTER == 4).Max(x => x.NF_DistanceTo4);
        //    NF_Spread_5 = TrainData.Where(y => y.NF_CLUSTER == 5).Max(x => x.NF_DistanceTo5);

        //    UF_Spread_1 = TrainData.Where(y => y.UF_CLUSTER == 1).Max(x => x.UF_DistanceTo1);
        //    UF_Spread_2 = TrainData.Where(y => y.UF_CLUSTER == 2).Max(x => x.UF_DistanceTo2);


        //    int test  = TrainData.ToList<Dots>().Where(x => x.UF_CLUSTER == 1).ToList().Count;
        //    int test2 = TrainData.ToList<Dots>().Where(x => x.UF_CLUSTER == 2).ToList().Count;

        //    int test3 = TrainData.ToList<Dots>().Where(x => x.NF_CLUSTER == 1).ToList().Count;
        //    int test4 = TrainData.ToList<Dots>().Where(x => x.NF_CLUSTER == 2).ToList().Count;
        //    int test5 = TrainData.ToList<Dots>().Where(x => x.NF_CLUSTER == 3).ToList().Count;
        //    int test6 = TrainData.ToList<Dots>().Where(x => x.NF_CLUSTER == 4).ToList().Count;
        //    int test7 = TrainData.ToList<Dots>().Where(x => x.NF_CLUSTER == 5).ToList().Count;


        //    return TrainData;
        //}


    }
}
