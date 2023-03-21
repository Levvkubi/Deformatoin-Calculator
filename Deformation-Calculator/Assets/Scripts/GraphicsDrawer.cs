using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsDrawer  : MonoBehaviour
{
    [SerializeField] private bool diffEdges;

    [Space]
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject edgePrefab;


    public void Draw(int npq, float[,] AKT, List<int> verticesIndx, List<int> edgesIndx, List<int> edgesDir, int lx = 1, int ly = 1, int lz = 1)
    {
        if (diffEdges)
            drawWithEdges(npq, AKT, verticesIndx, edgesIndx, edgesDir);
        else
            draw(npq, AKT);
    }
    private void draw(int npq, float[,] AKT)
    {
        for (int i = 0; i < npq; i++)
        {
            createPoint(pointPrefab,
                        new Vector3(AKT[0, i], AKT[1, i], AKT[2, i]),
                        Quaternion.identity,
                        $"Point {i + 1}");
        }
    }

    private void drawWithEdges(int npq, float[,] AKT, List<int> verticesIndx, List<int> edgesIndx, List<int> edgesDir, int lx = 1, int ly = 1, int lz = 1)
    {
        foreach (var i in verticesIndx)
        {
            createPoint(pointPrefab,
                       new Vector3(AKT[0, i], -AKT[1, i], AKT[2, i]),
                       Quaternion.identity,
                       $"Vertex {i + 1}");
        }

        for (int i = 0; i < edgesIndx.Count; i++)
        {
            Quaternion rotation;
            float edgeLenht;
            switch (edgesDir[i])
            {
                case 1:
                    rotation = Quaternion.LookRotation(Vector3.right);
                    edgeLenht = lx;

                    break;
                case 2:
                    rotation = Quaternion.LookRotation(Vector3.up);
                    edgeLenht = ly;
                    break;
                case 3:
                    rotation = Quaternion.LookRotation(Vector3.forward);
                    edgeLenht = lz;
                    break;
                default:
                    rotation = Quaternion.identity;
                    edgeLenht = 1;
                    break;
            }

            var edge = createPoint(edgePrefab,
                       new Vector3(AKT[0, edgesIndx[i]], -AKT[1, edgesIndx[i]], AKT[2, edgesIndx[i]]),
                       rotation,
                       $"Edge {edgesIndx[i] + 1}");

            edge.transform.localScale = new Vector3(
                edge.transform.localScale.x,
                edge.transform.localScale.y,
                edge.transform.localScale.z * edgeLenht);
        }
    }

    private GameObject createPoint(GameObject prefab, Vector3 position, Quaternion rotation, string name)
    {
        GameObject res = Instantiate(prefab, transform, false);
        res.transform.localPosition = position;
        res.transform.localRotation = rotation;
        res.name = name;

        return res;
    }
}
