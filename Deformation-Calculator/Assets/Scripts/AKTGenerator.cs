using System.Collections.Generic;

public static class AKTGenerator
{
    public static double[,] GenerateAKT(int nx, int ny, int nz, float lx, float ly, float lz, out int npq, List<int> verticesIndx, List<int> edgesIndx, List<int> edgesDir)
    {
        int verticesCount = (nx + 1) * (ny + 1) * (nz + 1);
        int edgesCount = 3 * nx * ny * nz + 2 * nx * ny + 2 * nx * nz + 2 * ny * nz + nx + ny + nz;
        npq = verticesCount + edgesCount;

        double[,] AKT = new double[3, npq];

        verticesIndx.Clear();
        edgesIndx.Clear();
        edgesDir.Clear();

        int i = 0;

        for (int z = 0; z <= nz; z++)
        {
            for (int y = 0; y <= ny; y++)
            {
                for (int x = 0; x <= nx; x++)
                {
                    verticesIndx.Add(i);
                    addPoint(x, y, z, ref i);

                    if (x < nx)
                    {
                        edgesIndx.Add(i);
                        edgesDir.Add(1);
                        addPoint(x + 0.5f, y, z, ref i);
                    }
                }
                if (y < ny)
                {
                    for (int x = 0; x <= nx; x++)
                    {
                        edgesIndx.Add(i);
                        edgesDir.Add(2);
                        addPoint(x, y + 0.5f, z, ref i);
                    }
                }
            }
            if (z < nz)
            {
                for (int y = 0; y <= ny; y++)
                {
                    for (int x = 0; x <= nx; x++)
                    {
                        edgesIndx.Add(i);
                        edgesDir.Add(3);
                        addPoint(x, y, z + 0.5f, ref i);
                    }
                }
            }
        }

        void addPoint(float x, float y, float z, ref int i)
        {
            AKT[0, i] = x * lx;
            AKT[1, i] = y * ly;
            AKT[2, i] = z * lz;
            i++;
        }

        return AKT;

    }
    //public static double[,] GenerateAKT(int nx, int ny, int nz, float lx, float ly, float lz, out int npq, int[,] edges)
    //{
    //    int verticesCount = (nx + 1) * (ny + 1) * (nz + 1);
    //    int edgesCount = 3 * nx * ny * nz + 2 * nx * ny + 2 * nx * nz + 2 * ny * nz + nx + ny + nz;
    //    npq = verticesCount + edgesCount;

    //    double[,] AKT = new double[3, npq];

    //    edges = new int[edgesCount,3];

    //    int i = 0;

    //    for (int z = 0; z <= nz; z++)
    //    {
    //        for (int y = 0; y <= ny; y++)
    //        {
    //            for (int x = 0; x <= nx; x++)
    //            {
    //                verticesIndx.Add(i);
    //                addPoint(x, y, z, ref i);

    //                if (x < nx)
    //                {
    //                    edgesIndx.Add(i);
    //                    edgesDir.Add(1);
    //                    addPoint(x + 0.5f, y, z, ref i);
    //                }
    //            }
    //            if (y < ny)
    //            {
    //                for (int x = 0; x <= nx; x++)
    //                {
    //                    edgesIndx.Add(i);
    //                    edgesDir.Add(2);
    //                    addPoint(x, y + 0.5f, z, ref i);
    //                }
    //            }
    //        }
    //        if (z < nz)
    //        {
    //            for (int y = 0; y <= ny; y++)
    //            {
    //                for (int x = 0; x <= nx; x++)
    //                {
    //                    edgesIndx.Add(i);
    //                    edgesDir.Add(3);
    //                    addPoint(x, y, z + 0.5f, ref i);
    //                }
    //            }
    //        }
    //    }

    //    void addPoint(float x, float y, float z, ref int i)
    //    {
    //        AKT[0, i] = x * lx;
    //        AKT[1, i] = y * ly;
    //        AKT[2, i] = z * lz;
    //        i++;
    //    }

    //    return AKT;

    //}


}
