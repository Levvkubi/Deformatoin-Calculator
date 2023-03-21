using UnityEngine;

public static class NTCalculator
{
    public static int[,] CalculateNT(int nx, int ny, int nz, out int nel)
    {
        nel = nx * ny * nz;
        int[,] NT = new int[20, nel];
        int p = 0;
        int currx = nx;
        int curry = ny;

        for (int i = 0; i < nel; i++)
        {
            int f1 = funk1(nx, ny);
            int f2 = funk2(nx, currx);
            int f3 = funk3(nx, ny, currx, curry);
            NT[0, i] = p;
            NT[1, i] = p + 2;
            NT[2, i] = p + 3 * nx + 4;
            NT[3, i] = p + 3 * nx + 2;
            NT[4, i] = f1 + NT[0, i];
            NT[5, i] = f1 + NT[1, i];
            NT[6, i] = f1 + NT[2, i];
            NT[7, i] = f1 + NT[3, i];
            NT[8, i] = p + 1;
            NT[9, i] = f2 + p + 1;
            NT[10, i] = NT[3, i] + 1;
            NT[11, i] = f2 + p;
            NT[12, i] = f3 + p;
            NT[13, i] = NT[12, i] + 1;
            NT[14, i] = NT[13, i] + nx + 1;
            NT[15, i] = NT[12, i] + nx + 1;
            NT[16, i] = f1 + NT[8, i];
            NT[17, i] = f1 + f2 + p + 1;
            NT[18, i] = NT[7, i] + 1;
            NT[19, i] = NT[17, i] - 1;

            p += 2;
            currx--;
            if (currx <= 0)
            {
                p += nx + 2;
                currx = nx;
                curry--;
                if (curry <= 0)
                {
                    curry = ny;
                    p += (nx + 1) * (ny + 1) + 1 + 2 * nx;
                }
            }
        }

        return NT;
    }

    private static int funk1(int nx, int ny)
    {
        return 4 * nx * ny + 3 * nx + 3 * ny + 2;
    }

    private static int funk2(int nx, int currx)
    {
        return nx + currx + 1;
    }

    private static int funk3(int nx, int ny, int currx, int curry)
    {
        return 2 * nx * curry + nx * ny + nx + ny + currx + curry + 1;
    }

    public static int CalcualteNg(int[,] NT, int nel)
    {
        int[] NTMaxLen = new int[nel];
        int maxDiff = 0;
        for (int i = 0; i < nel; i++)
        {
            int currMax = NT[0, i];
            int currMin = NT[0, i];
            for (int j = 1; j < 20; j++)
            {
                if (currMax < NT[j, i])
                    currMax = NT[j, i];
                else if (currMin > NT[j, i])
                    currMin = NT[j, i];
            }
            int currDiff = currMax - currMin;
            NTMaxLen[i] = currDiff;
            if (maxDiff < currDiff)
                maxDiff = currDiff;
        }

        return 3 * (maxDiff + 1);
    }

    public static void logNT(int[,] NT, int nel)
    {
        for (int i = 0; i < nel; i++)
        {
            string res = $"{i + 1} : ";
            for (int j = 0; j < 20; j++)
            {
                res += $", {j + 1}-{ NT[j, i] + 1}";
            }
            Debug.Log(res);
        }
    }
}
