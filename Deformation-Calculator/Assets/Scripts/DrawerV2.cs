using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerV2 : MonoBehaviour
{
    public void Draw(double[,] AKT,int[,] NT,Color color)
    {
        for (int i = 0; i < 8; i++)
        {
            drowLine(0, 8, 1, AKT, NT, i);
            drowLine(1, 9, 2, AKT, NT, i);
            drowLine(2, 10, 3, AKT, NT, i);
            drowLine(3, 11, 0, AKT, NT, i);
            drowLine(0, 12, 4, AKT, NT, i);
            drowLine(1, 13, 5, AKT, NT, i);
            drowLine(2, 14, 6, AKT, NT, i);
            drowLine(3, 15, 7, AKT, NT, i);
            drowLine(4, 16, 5, AKT, NT, i);
            drowLine(5, 17, 6, AKT, NT, i);
            drowLine(6, 18,7, AKT, NT, i);
            drowLine(7, 19, 4, AKT, NT, i);
        }
        
    }
    void drowLine(int p1, int p2, int p3, double[,] AKT, int[,] NT,int i)
    {
        GameObject lineObj = new GameObject("Line");
        LineRenderer line = lineObj.AddComponent<LineRenderer>();
        line.positionCount = 3;
        line.SetWidth(0.05f, 0.05f);
        line.SetPosition(0,
            new Vector3(
                (float)AKT[0, NT[p1, i]],
                (float)AKT[1, NT[p1, i]],
                (float)AKT[2, NT[p1, i]]
                ));
        line.SetPosition(1,
            new Vector3(
                (float)AKT[0, NT[p2, i]],
                (float)AKT[1, NT[p2, i]],
                (float)AKT[2, NT[p2, i]]
                ));
        line.SetPosition(2,
            new Vector3(
                (float)AKT[0, NT[p3, i]],
                (float)AKT[1, NT[p3, i]],
                (float)AKT[2, NT[p3, i]]
                ));
    }
}
