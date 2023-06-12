using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DFITNCalculator 
{
    static int[,] sidePoints = new int[,] { {-1, -1}, { 1, -1 }, { 1, 1 }, { -1, 1 },
                                    { 0, -1 }, { 1, 0 },  {0,1}, {-1,0} };
    public static double[,,] CalculateFITN()
    {
        double[,,] FITN = new double[8,2,8];

        double sq = Mathf.Sqrt(0.6f);
        int j = 0;

        for (int n = -1; n <= 1; n++)
        {
            for (int t = -1; t <= 1; t++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (i < 4)
                    {
                        FITN[j, 0, i] = fiVerticle(n * sq, t * sq, sidePoints[i, 0], sidePoints[i, 1]);
                        FITN[j, 1, i] = fiVerticle(n * sq, t * sq, sidePoints[i, 0], sidePoints[i, 1]);
                    }
                    else if(i == 5 || i == 7)
                    {
                        FITN[j, 0, i] = fiEdge(n * sq, t * sq, sidePoints[i, 0], sidePoints[i, 1]);
                        FITN[j, 1, i] = fiEdge(t * sq, n * sq, sidePoints[i, 1], sidePoints[i, 0]);
                    }
                }

                j++;
            }
        }

        return FITN;
    }
    public static double[,,] CalculateDFITN()
    {
        double[,,] DFITN = new double[9,2,8];

        double sq = Mathf.Sqrt(0.6f);
        int j = 0;

        return DFITN;
    }

    private static double fiVerticle(double x, double xi, double y, double yi)
    {
        return 0.25 * (1 + x * xi) * (1 + y * yi) * (x * xi + y * yi - 1);
    }
    private static double fiEdge(double x, double xi, double y, double yi)
    {
        return 0.5 * (1 - x * x) * (1 + y * yi);
    }

    public static double[,,] CalculateFi()
    {
        return new double[1,1,1];
    }
}
