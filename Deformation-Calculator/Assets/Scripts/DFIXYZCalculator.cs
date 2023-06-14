public class DFIXYZCalculator
{
    public static double[][,,] CalculateDFIXYZ(double[,,] dfiabg, double[][,,] dj)
    {
        double[][,,] dfixyz = new double[dj.Length][,,];
        for (int i = 0; i < dj.Length; i++)
        {
            dfixyz[i] = calculateCurrDFIXYZ(dfiabg, dj[i]);
        }

        return dfixyz;
    }
    private static double[,,] calculateCurrDFIXYZ(double[,,] dfiabg, double[,,] dj)
    {
        double[,,] dfixyz = new double[27, 3, 20];

        for (int i = 0; i < 27; i++)
        {
            double[,] currDj = new double[3, 3];

            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    currDj[j, k] = dj[i, j, k];
                }
            }

            for (int j = 0; j < 20; j++)
            {
                double[] currABG = { dfiabg[i, 0, j], dfiabg[i, 1, j], dfiabg[i, 2, j] };
                var curXYZ = KramarMethod.CalculateSLAR(currDj, currABG);

                for (int l = 0; l < 3; l++)
                {
                    dfixyz[i, l, j] = curXYZ[l];
                }
            }
        }

        return dfixyz;
    }
}
