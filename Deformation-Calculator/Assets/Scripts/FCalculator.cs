using System;
using System.Linq;

public class FCalculator
{
    public static double[] GetF(int[,] NT, int nX_amount, int nY_amount, int nZ_amount, double[,] AKT, int nel, double Pn)
    {
        double[,] EtaiTaui = new double[,]
        {
                {-1, 1, 1, -1, 0, 1, 0, -1},
                {-1, -1, 1, 1, -1, 0, 1, 0}
        };
        double[,] modEtaiTaui = new double[EtaiTaui.GetLength(1), EtaiTaui.GetLength(0)];

        var DPSITE = DPSITECalculator.CalculateDPSITE();

        for (int i = 0; i < EtaiTaui.GetLength(0); i++)
        {
            for (int j = 0; j < EtaiTaui.GetLength(1); j++)
            {
                modEtaiTaui[j, i] = Math.Round(EtaiTaui[i, j]);
            }
        }

        int[,] ZP = DeformationCalculator.GetZP();

        double[] EtaTau = new double[] { -Math.Sqrt(0.6), 0, Math.Sqrt(0.6) };
        double[] c = new double[] { 5 / 9.0, 8 / 9.0, 5 / 9.0 };

        double[] F = new double[AKT.GetLength(1) * 3];

        for (int i = 0; i < ZP.GetLength(0); i++)
        {
            double[] FE = new double[60];
            double[,] XYZ_ZP = new double[8, 3];

            int number = (int)ZP[i, 0];
            int face = (int)ZP[i, 1];
            int[] face_points = DeformationCalculator.SideToPoints(ZP[i, 0], ZP[i, 1], NT);
            int[] NTpoints = new int[face_points.Length];

            for (int j = 0; j < NTpoints.Length; j++)
            {
                NTpoints[j] = NT[face_points[j], number];
            }

            for (int j = 0; j < NTpoints.Length; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    XYZ_ZP[j, k] = AKT[k, NTpoints[j]];
                }
            }

            double[,,] DPSIXYZ = DPSITECalculator.CalculateDPSIXYZ(XYZ_ZP, DPSITE);
            int points_iter = 0;

            for (int k = 0; k < 20; k++)
            {
                if (face_points.Contains(k))
                {
                    int counter = 0;

                    var tau_index = 0;
                    foreach (double m in c)
                    {
                        var eta_index = 0;
                        foreach (double n in c)
                        {
                            double eta_i = modEtaiTaui[points_iter, 0];
                            double tau_i = modEtaiTaui[points_iter, 1];
                            double phi_k = 0.0;

                            if (points_iter < 4)
                            {
                                phi_k = 0.25 * (1 + EtaTau[eta_index] * eta_i) * (1 + EtaTau[tau_index] * tau_i) * (EtaTau[eta_index] * eta_i + EtaTau[tau_index] * tau_i - 1);
                            }
                            else if (points_iter == 4 || points_iter == 6)
                            {
                                phi_k = 0.5 * (1 - Math.Pow(EtaTau[eta_index], 2)) * (1 + EtaTau[tau_index] * tau_i);
                            }
                            else if (points_iter == 5 || points_iter == 7)
                            {
                                phi_k = 0.5 * (1 - Math.Pow(EtaTau[tau_index], 2)) * (1 + EtaTau[eta_index] * eta_i);
                            }

                            FE[k] += m * n * Pn * (DPSIXYZ[counter, 1, 0] * DPSIXYZ[counter, 2, 1] - DPSIXYZ[counter, 2, 0] * DPSIXYZ[counter, 1, 1]) * phi_k;
                            FE[20 + k] += m * n * Pn * (DPSIXYZ[counter, 2, 0] * DPSIXYZ[counter, 0, 1] - DPSIXYZ[counter, 0, 0] * DPSIXYZ[counter, 2, 1]) * phi_k;
                            FE[40 + k] += m * n * Pn * (DPSIXYZ[counter, 0, 0] * DPSIXYZ[counter, 1, 1] - DPSIXYZ[counter, 1, 0] * DPSIXYZ[counter, 0, 1]) * phi_k;

                            counter++;
                            eta_index++;
                        }

                        tau_index++;
                    }

                    points_iter++;
                }
            }

            var itemsLength = NT.GetLength(0);
            var items = new int[itemsLength];

            for (int j = 0; j < itemsLength; j++)
            {
                items[j] = NT[j, number];
            }

            for (int item = 0; item < 60; item++)
            {
                int row = 3 * items[item % 20] + (item / 20);
                F[row] += FE[item];
            }
        }

        return F;
    }
}