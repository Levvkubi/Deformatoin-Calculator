using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DeformationCalculator : MonoBehaviour
{
    [SerializeField] private int el;//тимчасова змінна для перевірки
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
    [SerializeField] private double P = 100;
    [SerializeField] private double E = 1;
    [SerializeField] private double V = 0.3;
    [SerializeField] private double Lambda;
    [SerializeField] private double Mu;

    [Space]
    [SerializeField] private bool showNT;

    [Space]
    [SerializeField] private List<Vector2> ZP;
    [SerializeField] private List<Vector2> ZU;

    [Space]
    [SerializeField] private int npq;
    [SerializeField] private double[,] AKT;
    [SerializeField] private int nel;
    [SerializeField] private int[,] NT;
    [SerializeField] private int ng;
    [SerializeField] private int[] NTMaxLen;
    [SerializeField] private double[,,] DFIABG;
    [SerializeField] private double[][,,] DJ;
    [SerializeField] private double[][,,] DFIXYZ;
    [SerializeField] private double[,] MGE;

    private List<int> verticesIndx;
    private List<int> edgesIndx;
    private List<int> edgesDir;

    private static float lx, ly, lz;

    private GraphicsDrawer drawer;
    public DrawerV2 drawerV2;
    void Start()
    {
        Lambda = E / ((1 + V) * (1 - 2 * V));
        Mu = E / (2 * (1 + V));
        Debug.Log(1); 

        drawer = GetComponent<GraphicsDrawer>();
        //drawerV2 = GetComponent<DrawerV2>();
        Debug.Log(2);


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
        Debug.Log("AKT");

        NT = NTCalculator.CalculateNT(nx, ny, nz, out nel);
        Debug.Log("NT");

        ng = NTCalculator.CalcualteNg(NT, nel);
        Debug.Log("NG");

        if (showNT)
            NTCalculator.logNT(NT, nel);

        //drawerV2.Draw( AKT, NT, Color.green);

        Debug.Log("draw");
        drawer.Draw(npq, AKT, verticesIndx, edgesIndx, edgesDir, lx, ly, lz);

        DFIABG = DFIABGCalculator.CalculateDFIABG();
        DJ = DJCalculator.CalculateDJ(AKT, NT, nel, DFIABG);

        DFIXYZ = DFIXYZCalculator.CalculateDFIXYZ(DFIABG, DJ);
        //for (int l = 0; l < 10; l++)
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        string r = string.Empty;
        //        for (int j = 0; j < 3; j++)
        //        {
        //            r += Math.Round(DFIXYZ[j, i, l],5);
        //            r += "\t";
        //        }

        //        Debug.Log(r);
        //    }
        //}
        double[][] mge;
        MGE = MGECalculator.CalculateMGE(DFIXYZ, DJ, AKT, NT, GetArrFromList(ZU), npq, nel, Lambda, V, Mu, out mge);
        //writeMatrixIntoFile(MGE);
        //writeMatrixIntoFile(mge);

        var F = FCalculator.CalculateF(NT, GetArrFromList(ZP), nx, ny, nz, AKT, nel, P);
        var MG_Matrix = Matrix<double>.Build.DenseOfArray(MGE);
        var F_Matrix = Matrix<double>.Build.DenseOfColumns(new List<double[]> { F });

        var U = MG_Matrix.Solve(F_Matrix).ToColumnArrays()[0];
        var U_SplittedByAxis = Make2DArray(U, U.Length / 3, 3);

        var U_Transposed = Transpose(U_SplittedByAxis);

        var AKT_Matrix = Matrix<double>.Build.DenseOfArray(AKT);

        var U_Matrix = Matrix<double>.Build.DenseOfArray(U_Transposed);

        var res = AKT_Matrix.Add(U_Matrix).ToArray();
        //drawer.Draw(npq, res, verticesIndx, edgesIndx, edgesDir, lx, ly, lz);
        drawerV2.Draw(res, NT, Color.green);


        Debug.Log("gg");
    }
    public static int[,] GetArrFromList(List<Vector2> list)////////////////
    {
        int[,] res = new int[list.Count, 2];
        for (int i = 0; i < list.Count; i++)
        {
            res[i, 0] = (int)list[i].x;
            res[i, 1] = (int)list[i].y;
        }
        return res;
    }
    public static void writeMatrixIntoFile(double[][] Arr)
    {
        string res = "      ";

        for (int i = 0; i < Arr.GetLength(0); i++)
        {
            if (i < 10)
                res += "      " + i.ToString();
            else
                res += "     " + i.ToString();
        }

        res += "\n";

        for (int i = 0; i < Arr.GetLength(0); i++)
        {
            res += i.ToString() + "\t";
            for (int j = 0; j < Arr[i].Length; j++)
            {
                double data = Math.Round(Arr[i][j], 2);

                if (data >= 0)
                    res += "  ";
                else
                    res += " ";

                res += $"{data:f2} ";

            }
            res += "\n";
            res += "\n";
        }
        File.WriteAllText(@"D:\Matrixcc.txt", res);
    }
    public static void writeMatrixIntoFile(double[,] Arr)
    {
        string res = "      ";

        for (int i = 0; i < Arr.GetLength(0); i++)
        {
            if (i < 10)
                res += "      " + i.ToString();
            else
                res += "     " + i.ToString();
        }

        res += "\n";

        for (int i = 0; i < Arr.GetLength(0); i++)
        {
            res += i.ToString() + "\t";
            for (int j = 0; j < Arr.GetLength(1); j++)
            {
                double data = Math.Round(Arr[i, j], 2);

                if (data >= 0)
                    res += "  ";
                else
                    res += " ";

                res += $"{data:f2} ";

            }
            res += "\n";
            res += "\n";
        }
        File.WriteAllText(@"D:\Matrix.txt", res);
    }
    public static int[] SideToPoints(int elNT, int side, int[,] NT)
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
    public static T[,] Make2DArray<T>(T[] input, int height, int width)
    {
        T[,] output = new T[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }

        return output;
    }
    public static double[,] Transpose(double[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var columns = matrix.GetLength(1);

        var result = new double[columns, rows];

        for (var c = 0; c < columns; c++)
        {
            for (var r = 0; r < rows; r++)
            {
                result[c, r] = matrix[r, c];
            }
        }

        return result;
    }
}
