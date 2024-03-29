using System;
using System.IO;
using UnityEngine;

public static class DFIABGCalculator
{
    private static readonly int[,] bigBoxPoints = {{-1,1,-1}, {1,1,-1}, {1,-1,-1}, {-1,-1,-1},
                                   {-1,1,1}, {1,1,1}, {1,-1,1}, {-1,-1,1 },
                                   {0,1,-1}, {1,0,-1}, {0,-1,-1}, {-1,0,-1},
                                   {-1,1,0}, {1,1,0}, {1,-1,0}, {-1,-1,0},
                                   {0,1,1}, {1,0,1}, {0,-1,1}, {-1,0,1}};


    public static double[,,] CalculateDFIABG()
    {
        double[,,] DFIABG = new double[27, 3, 20];

        double sq = Mathf.Sqrt(0.6f);
        int j = 0;

        for (int a = -1; a <= 1; a++)
        {
            for (int b = -1; b <= 1; b++)
            {
                for (int g = -1; g <= 1; g++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        DFIABG[j, 0, i] = verticleA(a * sq, b * sq, g * sq, bigBoxPoints[i, 0], bigBoxPoints[i, 1], bigBoxPoints[i, 2]);
                        DFIABG[j, 1, i] = verticleB(a * sq, b * sq, g * sq, bigBoxPoints[i, 0], bigBoxPoints[i, 1], bigBoxPoints[i, 2]);
                        DFIABG[j, 2, i] = verticleG(a * sq, b * sq, g * sq, bigBoxPoints[i, 0], bigBoxPoints[i, 1], bigBoxPoints[i, 2]);
                    }
                    for (int i = 8; i < 20; i++)
                    {
                        DFIABG[j, 0, i] = eadgeA(a * sq, b * sq, g * sq, bigBoxPoints[i, 0], bigBoxPoints[i, 1], bigBoxPoints[i, 2]);
                        DFIABG[j, 1, i] = eadgeB(a * sq, b * sq, g * sq, bigBoxPoints[i, 0], bigBoxPoints[i, 1], bigBoxPoints[i, 2]);
                        DFIABG[j, 2, i] = eadgeG(a * sq, b * sq, g * sq, bigBoxPoints[i, 0], bigBoxPoints[i, 1], bigBoxPoints[i, 2]);
                    }

                    j++;
                }
            }
        }

        return DFIABG;
    }

    private static double verticleA(double a, double b, double g, double ai, double bi, double gi)
    {
        return pohidnaVerticle(a, b, g, ai, bi, gi);
    }

    private static double verticleB(double a, double b, double g, double ai, double bi, double gi)
    {
        return pohidnaVerticle(b, a, g, bi, ai, gi);
    }

    private static double verticleG(double a, double b, double g, double ai, double bi, double gi)
    {
        return pohidnaVerticle(g, a, b, gi, ai, bi);
    }

    private static double pohidnaVerticle(double shucana, double x1, double x2, double shucanai, double x1i, double x2i)
    {
        return (1f / 8f) * (1 + x1 * x1i) * (1 + x2 * x2i) * (shucanai * (2 * shucana * shucanai + x1 * x1i + x2 * x2i - 1));
    }

    private static double eadgeA(double a, double b, double g, double ai, double bi, double gi)
    {
        return pohidnaEadge(a, b, g, ai, bi, gi);
    }

    private static double eadgeB(double a, double b, double g, double ai, double bi, double gi)
    {
        return pohidnaEadge(b, a, g, bi, ai, gi);
    }

    private static double eadgeG(double a, double b, double g, double ai, double bi, double gi)
    {
        return pohidnaEadge(g, a, b, gi, ai, bi);
    }

    private static double pohidnaEadge(double a, double b, double g, double ai, double bi, double gi)
    {
        return (1f / 4f) * (b * bi + 1) * (g * gi + 1) * (ai * (-bi * bi * gi * gi * a * a - ai * ai * bi * bi * g * g - ai * ai * gi * gi * b * b + 1) - 2 * bi * bi * gi * gi * a * (a * ai + 1));
    }

    public static void writeIntoFile(double[,,] DFIABG)
    {
        string res = string.Empty;
        for (int i = 0; i < 27; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 20; k++)
                {
                    res += Math.Round(DFIABG[i, j, k], 5).ToString() + "\t";
                }
                res += "\n";
            }
            res += "\n";
        }

        File.WriteAllText(@"D:\dfi.txt", res);
    }
}
