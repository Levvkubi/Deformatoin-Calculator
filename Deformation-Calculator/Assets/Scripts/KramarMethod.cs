public static class KramarMethod 
{
    public static double[] CalculateSLAR(double[,,] dj,int el, double[] constants)
    {

        double[,] coefficients = new double[3,3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                coefficients[i, j] = dj[el,i,j];
            }
        }

        double detA = Determinant(coefficients);

        double[] solutions = new double[3];

        for (int i = 0; i < 3; i++)
        {
            double[,] Ai = ReplaceColumn(coefficients, i, constants);
            double detAi = Determinant(Ai);
            solutions[i] = detAi / detA;
        }

        return solutions;
    }

    static double Determinant(double[,] matrix)
    {
        int n = matrix.GetLength(0);

        if (n == 1)
        {
            return matrix[0, 0];
        }

        double det = 0;

        for (int j = 0; j < n; j++)
        {
            double[,] submatrix = Submatrix(matrix, 0, j);
            double sign = (j % 2 == 0) ? 1 : -1;
            det += sign * matrix[0, j] * Determinant(submatrix);
        }

        return det;
    }

    static double[,] Submatrix(double[,] matrix, int rowToRemove, int columnToRemove)
    {
        int n = matrix.GetLength(0);
        double[,] submatrix = new double[n - 1, n - 1];

        for (int i = 0, k = 0; i < n; i++)
        {
            if (i == rowToRemove)
            {
                continue;
            }

            for (int j = 0, l = 0; j < n; j++)
            {
                if (j == columnToRemove)
                {
                    continue;
                }

                submatrix[k, l] = matrix[i, j];
                l++;
            }

            k++;
        }

        return submatrix;
    }

    static double[,] ReplaceColumn(double[,] matrix, int columnIndex, double[] columnValues)
    {
        int n = matrix.GetLength(0);
        double[,] newMatrix = new double[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (j == columnIndex)
                {
                    newMatrix[i, j] = columnValues[i];
                }
                else
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }
        }

        return newMatrix;
    }
}
