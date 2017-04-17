using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupRBF();

        }
        public void SetupRBF()
        {
            ExtractData.SetDataTrain();
            ExtractData.SetDataValidation();
            Random rrr = new Random();

            //Clustering ClusterData = new Clustering();
            //ClusterData.Cluster();

            //StringBuilder str = new StringBuilder();
            //str.AppendLine("X\tY\t");
            //foreach (Dots x in ExtractData.GetDataValidation())
            //{
            //    str.AppendLine(String.Format("{0}\t{1}\t", x.X, x.Y));
            //}
            //textBox1.Text = str.ToString();



            for (int l = 0; l < 100; l++)
            {
                StringBuilder str = new StringBuilder();
                StringBuilder str2 = new StringBuilder();
                for (int k = 100; k < 10001; k *= 10)
                {
                    for (int i = 1; i < 11; i++) // Cluster Number Iteration
                    {
                        string Key = MD5Sifrele(DateTime.Now.ToString() + rrr.Next().ToString());
                        int cluster_count = i;
                        RBF rbf = new RBF(cluster_count, k);
                        str.AppendLine(Key);
                        str.AppendLine(String.Format("RBF with {0} Epoch, {1} Training Coefficient, {2} Clusters", k, 0.1, i));
                        str.AppendLine("X\tY\tResult\tCluster\t");
                        foreach (Dots x in ExtractData.GetDataValidation().OrderBy(x => x.BelongingCluster[cluster_count - 1]))
                        {
                            str.AppendLine(String.Format("{0}\t{1}\t{2}\t{3}\t", x.X, x.Y, x.outY, x.BelongingCluster[cluster_count - 1]));
                        }
                        str.AppendLine(""); str.AppendLine(""); str.AppendLine("");
                        str2.AppendLine(Key);
                        str2.AppendLine(rbf.ToString());
                    }
                }
                System.IO.StreamWriter file = new System.IO.StreamWriter("d:\\Results\\OutputRBF_Stats_TestRun_#" + (l + 1) + ".txt");
                file.Write(str2.ToString());
                file.Close();

                System.IO.StreamWriter file2 = new System.IO.StreamWriter("d:\\Results\\OutputRBF_Predictions_TestRun_#" + (l + 1) + ".txt");
                file2.Write(str.ToString());
                file2.Close();
            }

            //Mean Error - Mean Square Error Stats Exporter
            //StringBuilder str = new StringBuilder();
            //str.AppendLine("ID\tEpoch\tCluster\tMean Error\tMean Square Error\t");
            //int id = 0;
            //for (int l = 0; l < 100; l++)
            //{

            //    for (int k = 100; k < 10001; k *= 10)
            //    {
            //        for (int i = 1; i < 11; i++) // Cluster Number Iteration
            //        {
            //            id++;
            //            //string Key = MD5Sifrele(DateTime.Now.ToString() + rrr.Next().ToString());
            //            int cluster_count = i;
            //            RBF rbf = new RBF(cluster_count, k);
            //            str.AppendLine(String.Format("{0}\t{1}\t{2}\t{3}\t{4}",id, k, cluster_count, rbf.MeanError.ToString("#.##"), rbf.SumOfSquareErrorMean.ToString("#.##")));
            //        }
            //    }

            //}

            //System.IO.StreamWriter file2 = new System.IO.StreamWriter("d:\\Results\\OutputRBF_Predictions_General_Plot" + ".txt");
            //file2.Write(str.ToString());
            //file2.Close();

            //textBox1.Text = str.ToString() ;
            //textBox2.Text = str2.ToString();




        }

        public  string MD5Sifrele(string metin)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] btr = Encoding.UTF8.GetBytes(metin);
            btr = md5.ComputeHash(btr);


            StringBuilder sb = new StringBuilder();
            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }


    }
}
