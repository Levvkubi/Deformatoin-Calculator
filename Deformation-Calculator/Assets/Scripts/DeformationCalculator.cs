using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationCalculator : MonoBehaviour
{

    [Space]
    [SerializeField] private List<Vector2> ZP;
    [SerializeField] private List<Vector2> ZU;

    [Space]
    public int npq;
    public float[,] AKT;
    public int nel;
    public int[,] NT;
    public int[] NTMaxLen;
    public int[,,] DFIABG;
    public int ng;

    private AKTGenerator AKTG;
    private GraphicsDrawer  visualizer;
    void Start()
    {
        AKTG = GetComponent<AKTGenerator>();
        visualizer = GetComponent<GraphicsDrawer >();

        AKTG.Create();
        visualizer.Drow();
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
