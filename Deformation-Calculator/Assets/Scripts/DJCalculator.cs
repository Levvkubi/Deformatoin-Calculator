using System;
using System.IO;

public static class DJCalculator
{
    public static float[,,] CalculateDJ(float[,] AKT,int [,] NT, int el, float[,,] DFIABG)
    {
        float[,,] DJ = new float[27, 3, 3];

        for (int i = 0; i < 27; i++)
        {
            for (int a = 0; a < 3; a++)
            {
                for (int x = 0; x < 3; x++)
                {
                    float sum = 0;
                    for (int k = 0; k < 20; k++)
                    {
                        sum += AKT[x, NT[k, el]] * DFIABG[i, a, k];
                    }
                    DJ[i, a, x] = sum;
                }
            }
        }

        return DJ;
    }
    public static void writeIntoFile(float[,,] DJ)
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
        File.Create(@"D:\DJ.txt");
        File.WriteAllText(@"D:\DJ.txt", res);
    }
    public static float CalculateDeterminant3x3(float[,] matrix)
    {
        if(matrix.Length != 3 || matrix.LongLength != 3)
            throw new ArgumentException("Invalid matrix size");

        return matrix[0, 0] * matrix[1, 1] * matrix[2, 2] + matrix[0, 1] * matrix[1, 2] * matrix[2, 0] +
               matrix[0, 2] * matrix[1, 0] * matrix[2, 1] + matrix[0, 2] * matrix[1, 1] * matrix[2, 0] +
               matrix[0, 1] * matrix[1, 0] * matrix[2, 2] + matrix[0, 0] * matrix[1, 2] * matrix[2, 1];
    }
}
