using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationCalculator : MonoBehaviour
{
    [Space]
    [SerializeField] private int nx;
    [SerializeField] private int ny;
    [SerializeField] private int nz;

    [Space]
    [SerializeField] private bool useCustomProportins;

    [Space]
    [SerializeField] private int ax;
    [SerializeField] private int ay;
    [SerializeField] private int az;

    [Space]
    [SerializeField] private bool showNT;

    [Space]
    [SerializeField] private List<Vector2> ZP;
    [SerializeField] private List<Vector2> ZU;

    [Space]
    [SerializeField] private int npq;
    [SerializeField] private float[,] AKT;
    [SerializeField] private int nel;
    [SerializeField] private int[,] NT;
    [SerializeField] private int[] NTMaxLen;
    [SerializeField] private int[,,] DFIABG;
    [SerializeField] private int ng;
    [SerializeField] private float[,,] DJ;

    private List<int> verticesIndx;
    private List<int> edgesIndx;
    private List<int> edgesDir;

    private static float lx, ly, lz;

    private GraphicsDrawer  drawer;
    void Start()
    {
        drawer = GetComponent<GraphicsDrawer >();

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

        verticesIndx = new List<int>();
        edgesIndx = new List<int>();
        edgesDir = new List<int>();

        AKT = AKTGenerator.GenerateAKT(nx, ny, nz, lx, ly, lz, out npq, verticesIndx, edgesIndx, edgesDir);
        NT = NTCalculator.CalculateNT(nx, ny, nz, out nel);
        ng = NTCalculator.CalcualteNg(NT, nel);

        if (showNT)
            NTCalculator.logNT(NT, nel);


        drawer.Draw(npq, AKT, verticesIndx, edgesIndx, edgesDir, lx, ly, lz);


        // DJ = DJCalculator.CalculateDJ(AKT, NT, 0, DFIABG);
    }

    private int[] sideToPoints(int elNT, int side)
    {
        int[] points = new int[8];

        switch (side)
        {
            case 0:
                points[0] = NT[0, elNT];
                points[1] = NT[1, elNT];
                points[2] = NT[5, elNT];
                points[3] = NT[4, elNT];
                points[4] = NT[8, elNT];
                points[5] = NT[13, elNT];
                points[6] = NT[16, elNT];
                points[7] = NT[12, elNT];
                break;
            case 1:
                points[0] = NT[1, elNT];
                points[1] = NT[2, elNT];
                points[2] = NT[6, elNT];
                points[3] = NT[5, elNT];
                points[4] = NT[9, elNT];
                points[5] = NT[14, elNT];
                points[6] = NT[17, elNT];
                points[7] = NT[13, elNT];
                break;
            case 2:
                points[0] = NT[2, elNT];
                points[1] = NT[3, elNT];
                points[2] = NT[7, elNT];
                points[3] = NT[6, elNT];
                points[4] = NT[10, elNT];
                points[5] = NT[15, elNT];
                points[6] = NT[18, elNT];
                points[7] = NT[14, elNT]; ;
                break;
            case 3:
                points[0] = NT[3, elNT];
                points[1] = NT[0, elNT];
                points[2] = NT[4, elNT];
                points[3] = NT[7, elNT];
                points[4] = NT[11, elNT];
                points[5] = NT[12, elNT];
                points[6] = NT[19, elNT];
                points[7] = NT[15, elNT];
                break;
            case 4:
                points[0] = NT[0, elNT];
                points[1] = NT[1, elNT];
                points[2] = NT[2, elNT];
                points[3] = NT[3, elNT];
                points[4] = NT[8, elNT];
                points[5] = NT[9, elNT];
                points[6] = NT[10, elNT];
                points[7] = NT[11, elNT];
                break;
            case 5:
                points[0] = NT[4, elNT];
                points[1] = NT[5, elNT];
                points[2] = NT[6, elNT];
                points[3] = NT[7, elNT];
                points[4] = NT[16, elNT];
                points[5] = NT[17, elNT];
                points[6] = NT[18, elNT];
                points[7] = NT[19, elNT];
                break;
        }
        return points;
    }
}
