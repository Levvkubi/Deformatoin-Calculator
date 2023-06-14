using System;
using System.IO;
using UnityEngine;

public static class DJCalculator
{
    public static double[][,,] CalculateDJ(double[,] AKT, int[,] NT, int nel, double[,,] DFIABG)
    {
        double[][,,] DJ = new double[nel][,,];
        for (int i = 0; i < nel; i++)
        {
            DJ[i] = calculateCurrDJ(AKT, NT, i, DFIABG);
        }

        return DJ;
    }
    private static double[,,] calculateCurrDJ(double[,] AKT, int[,] NT, int el, double[,,] DFIABG)
    {
        double[,,] DJ = new double[27, 3, 3];

        for (int g = 0; g < 27; g++)
        {
            for (int a = 0; a < 3; a++)
            {
                for (int x = 0; x < 3; x++)
                {
                    double sum = 0;
                    for (int k = 0; k < 20; k++)
                    {
                        sum += AKT[x, NT[k, el]] * DFIABG[g, a, k];
                    }
                    DJ[g, a, x] = sum;
                }
            }
        }

        return DJ;
    }
    public static void writeIntoConsole(double[,,] DJ)
    {
        string res = string.Empty;
        for (int i = 0; i < 27; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    res += Math.Round(DJ[i, j, k], 5).ToString() + "\t";
                }
                Debug.Log(res);
                res = string.Empty;
            }
            Debug.Log(res);
        }
    }
    public static void writeIntoFile(double[,,] DJ)
    {
        string res = string.Empty;
        for (int i = 0; i < 27; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    res += Math.Round(DJ[i, j, k], 5).ToString() + "\t";
                }
                res += "\n";
            }
            res += "\n";
        }
        //File.Create(@"D:\DJ.txt");
        File.WriteAllText(@"D:\DJ.txt", res);
        //FileStream fileStream = new FileStream(@"D:\DJ.txt",
        //                               FileMode.OpenOrCreate,
        //                               FileAccess.ReadWrite,
        //                               FileShare.None);
        //fileStream.Write(res);
    }
    public static double CalculateDeterminant3x3(double[,,] matrix, int el)
    {
        //if(matrix.Length != 3 || matrix.LongLength != 3)
        //    throw new ArgumentException("Invalid matrix size");

        return matrix[el, 0, 0] * matrix[el, 1, 1] * matrix[el, 2, 2] + matrix[el, 0, 1] * matrix[el, 1, 2] * matrix[el, 2, 0] +
               matrix[el, 0, 2] * matrix[el, 1, 0] * matrix[el, 2, 1] + matrix[el, 0, 2] * matrix[el, 1, 1] * matrix[el, 2, 0] +
               matrix[el, 0, 1] * matrix[el, 1, 0] * matrix[el, 2, 2] + matrix[el, 0, 0] * matrix[el, 1, 2] * matrix[el, 2, 1];
    }
}
