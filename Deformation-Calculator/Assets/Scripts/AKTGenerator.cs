using System.Collections.Generic;
using UnityEngine;

public class AKTGenerator : MonoBehaviour
{
    public bool useCustomProportins;
    [SerializeField] private bool showNT;

    [Space]
    [SerializeField] private int nx;
    [SerializeField] private int ny;
    [SerializeField] private int nz;

    [Space]
    [SerializeField] private int ax;
    [SerializeField] private int ay;
    [SerializeField] private int az;

    [Space]
    public List<int> verticesIndx;
    public List<int> edgesIndx;
    public List<int> edgesDir;

    public float lx { get; private set; }
    public float ly { get; private set; }
    public float lz { get; private set; }

    private DeformationCalculator DC;

    public void Create()
    { 
        if(!DC)
            DC = GetComponent<DeformationCalculator>();

        if (useCustomProportins)
        {
            lx = ax / (float)nx;
            ly = ay / (float)ny;
            lz = az / (float)nz;
        }
        else
        {
            lx = 1;
            ly = 1;
            lz = 1;
        }

        createAKT();
        calculateNT();
        calcualteNg();

        if (showNT)
            logNT();
    }

    private void createArrays()
    {
        int verticesCount = (nx + 1)*(ny + 1)*(nz + 1);
        int edgesCount = 3 * nx * ny * nz + 2 * nx * ny + 2 * nx * nz + 2 * ny * nz + nx + ny + nz;
        DC.npq = verticesCount + edgesCount;

        DC.AKT = new float[3, DC.npq];

        verticesIndx = new List<int>();
        edgesIndx = new List<int>();
        edgesDir = new List<int>();
    }

    private void createAKT()
    {
        createArrays();

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
    }

    private void addPoint(float x,float y,float z,ref int i)
    {
        DC.AKT[0, i] = x * lx;
        DC.AKT[1, i] = y * ly;
        DC.AKT[2, i] = z * lz;
        i++;
    }

    private void calculateNT()
    {
        DC.nel = nx * ny * nz;
        DC.NT = new int[20, DC.nel];
        int p = 0;
        int currx = nx;
        int curry = ny;

        for (int i = 0; i < DC.nel; i++)
        {
            int f1 = funk1();
            int f2 = funk2(currx);
            int f3 = funk3(currx, curry);
            DC.NT[0, i] = p;
            DC.NT[1, i] = p + 2;
            DC.NT[2, i] = p + 3 * nx + 4;
            DC.NT[3, i] = p + 3 * nx + 2;
            DC.NT[4, i] = f1 + DC.NT[0, i];
            DC.NT[5, i] = f1 + DC.NT[1, i];
            DC.NT[6, i] = f1 + DC.NT[2, i];
            DC.NT[7, i] = f1 + DC.NT[3, i];
            DC.NT[8, i] = p + 1;
            DC.NT[9, i] = f2 + p + 1;
            DC.NT[10, i] = DC.NT[3, i] + 1;
            DC.NT[11, i] = f2 + p;
            DC.NT[12, i] = f3 + p;
            DC.NT[13, i] = DC.NT[12, i] + 1;
            DC.NT[14, i] = DC.NT[13, i] + nx + 1;
            DC.NT[15, i] = DC.NT[12, i] + nx + 1;
            DC.NT[16, i] = f1 + DC.NT[8, i];
            DC.NT[17, i] = f1 + f2 + p + 1;
            DC.NT[18, i] = DC.NT[7, i] + 1;
            DC.NT[19, i] = DC.NT[17, i] - 1;

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

    }

    private int funk1()
    {
        return 4 * nx * ny + 3 * nx + 3 * ny + 2;
    }

    private int funk2(int currx)
    {
        return nx + currx + 1;
    }

    private int funk3(int currx, int curry)
    {
        return 2 * nx * curry + nx * ny + nx + ny + currx + curry + 1;
    }

    private void calcualteNg()
    {
        DC.NTMaxLen = new int[DC.nel];
        int maxDiff = 0;
        for (int i = 0; i < DC.nel; i++)
        {
            int currMax = DC.NT[0,i];
            int currMin = DC.NT[0,i];
            for (int j = 1; j < 20; j++)
            {
                if(currMax < DC.NT[j,i])
                    currMax = DC.NT[j,i];
                else if(currMin > DC.NT[j,i])
                    currMin = DC.NT[j,i];
            }
            int currDiff = currMax - currMin;
            DC.NTMaxLen[i] = currDiff;
            if (maxDiff < currDiff) 
                maxDiff = currDiff;
        }

        DC.ng = 3 * (maxDiff + 1);
    }

    private void logNT()
    {
        for (int i = 0; i < DC.nel; i++)
        {
            string res = $"{i + 1} : ";
            for (int j = 0; j < 20; j++)
            {
                res += $", {j + 1}-{ DC.NT[j, i] + 1}";
            }
            Debug.Log(res);
        }
    }
}
