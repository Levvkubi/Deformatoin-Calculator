using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MGECalculator
{
    readonly static double[] C = { 5f / 9f, 8f / 9f, 5f / 9f };
    public static double[,] CalculateMGE(double[][,,] dfixyz, double[][,,] dj, double[,] akt, int[,] nt, int npq, int nel, double lambda, double V, double Mu, out double[][] cc)
    {
        int mgeSize = 3 * npq;
        double[,] mge = new double[mgeSize, mgeSize];
        double[][] currMge;
        for (int i = 0; i < nel; i++)
        {
            currMge = calculateCurrMGE(dfixyz[i], dj[i], lambda, V, Mu);
            for (int k = 0; k < 60; k++)
            {
                for (int l = k; l < 60; l++)
                {
                    int xmge = nt[k % 20, i]*3 + k/20; 
                    int ymge = nt[l % 20, i]*3 + l/20;
                    if(xmge > ymge)
                    {
                        int temp = xmge;
                        xmge = ymge;
                        ymge = temp;
                    }
                    mge[xmge, ymge] += currMge[k][l-k];
                }
            }
        }
        cc = calculateCurrMGE(dfixyz[0], dj[0], lambda, V, Mu);
        return mge;
    }
    //private int getIndex(int[,] nt,int ind)
    //{

    //}
    private static double[][] calculateCurrMGE(double[,,] dfixyz,double[,,] dj, double lambda, double V, double Mu)
    {
        double[][] mge = new double[60][];
        for (int i = 0; i < 60; i++)
            mge[i] = new double[60 - i];

        for (int p = 0; p < 60; p++)
        {
            mge[p] = new double[60 - p];
            int i = p % 20;
            int axis1 = p / 20;
            for (int l = p; l < 60; l++)
            {
                int j = l % 20;
                int axis2 = l / 20;
                double sum = 0;
                int gaus = 0;

                for (int m = 0; m < 3; m++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            double curr;
                            if (axis1 == axis2)
                            {
                                curr = lambda * (1 - V) * dfixyz[gaus, axis1, i] * dfixyz[gaus, axis2, j] +
                                    Mu * (dfixyz[gaus, (axis1 + 1) % 3, i] * dfixyz[gaus, (axis2 + 1) % 3, j]
                                    + dfixyz[gaus, (axis1 + 2) % 3, i] * dfixyz[gaus, (axis2 + 2) % 3, j]);
                            }
                            else
                            {
                                curr = lambda * V * dfixyz[gaus, axis1, i] * dfixyz[gaus, axis2, j]
                                    + Mu * dfixyz[gaus, axis2, i] * dfixyz[gaus, axis1, j];
                            }

                            curr *= C[m]; 
                            curr *= C[n]; 
                            curr *= C[k];

                            curr *= DJCalculator.CalculateDeterminant3x3(dj, gaus);

                            sum += curr; 

                            gaus++;
                        }
                    }
                }

                mge[p][l-p] = sum;
            }
        }

        return mge;
    }
}
