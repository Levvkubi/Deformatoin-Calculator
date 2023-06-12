using System;

public class DPSITECalculator
{
    public static double[,,] CalculateDPSITE()
    {
        double[,] EtaiTaui = new double[,]
        {
                {-1, 1, 1, -1, 0, 1, 0, -1},
                {-1, -1, 1, 1, -1, 0, 1, 0}
        };
        double[,] modEtaiTaui = new double[EtaiTaui.GetLength(1), EtaiTaui.GetLength(0)];

        for (int i = 0; i < EtaiTaui.GetLength(0); i++)
        {
            for (int j = 0; j < EtaiTaui.GetLength(1); j++)
            {
                modEtaiTaui[j, i] = Math.Round(EtaiTaui[i, j]);
            }
        }

        double[] EtaTau = new double[] { -Math.Sqrt(0.6), 0, Math.Sqrt(0.6) };
        int counter = 0;

        double[,,] DPSITE = new double[9, 2, 8];

        foreach (double tau in EtaTau)
        {
            foreach (double eta in EtaTau)
            {
                for (int i = 0; i < 8; i++)
                {
                    double eta_i = modEtaiTaui[i, 0];
                    double tau_i = modEtaiTaui[i, 1];

                    if (i == 4 || i == 6)
                    {
                        double dEta1 = -eta * (tau * tau_i + 1);
                        double dTau1 = 0.5 * (tau_i * (1 - Math.Pow(eta, 2)));
                        DPSITE[counter, 0, i] = dEta1;
                        DPSITE[counter, 1, i] = dTau1;
                        continue;
                    }

                    if (i == 5 || i == 7)
                    {
                        double dEta2 = 0.5 * (eta_i * (1 - Math.Pow(tau, 2)));
                        double dTau2 = -tau * (eta * eta_i + 1);
                        DPSITE[counter, 0, i] = dEta2;
                        DPSITE[counter, 1, i] = dTau2;
                        continue;
                    }

                    double dEta = 0.25 * (1 + tau * tau_i) * (2 * eta * Math.Pow(eta_i, 2) + eta_i * tau * tau_i);
                    double dTau = 0.25 * (1 + eta * eta_i) * (2 * tau * Math.Pow(tau_i, 2) + eta * eta_i * tau_i);

                    DPSITE[counter, 0, i] = dEta;
                    DPSITE[counter, 1, i] = dTau;
                }

                counter++;
            }
        }

        return DPSITE;
    }

    public static double[,,] CalculateDPSIXYZ(double[,] XYZ_ZP, double[,,] DPSITE)
    {
        double[,,] DPSIXYZ = new double[9, 3, 2];

        for (int j = 0; j < 9; j++)
        {
            for (int i = 0; i < XYZ_ZP.GetLength(0); i++)
            {
                DPSIXYZ[j, 0, 0] += XYZ_ZP[i, 0] * DPSITE[j, 0, i];  // dx/dE
                DPSIXYZ[j, 0, 1] += XYZ_ZP[i, 0] * DPSITE[j, 1, i];  // dx/dE

                DPSIXYZ[j, 1, 0] += XYZ_ZP[i, 1] * DPSITE[j, 0, i];  // dy/dE
                DPSIXYZ[j, 1, 1] += XYZ_ZP[i, 1] * DPSITE[j, 1, i];  // dy/dE

                DPSIXYZ[j, 2, 0] += XYZ_ZP[i, 2] * DPSITE[j, 0, i];  // dz/dE
                DPSIXYZ[j, 2, 1] += XYZ_ZP[i, 2] * DPSITE[j, 1, i];  // dz/dE
            }
        }

        return DPSIXYZ;
    }
}
