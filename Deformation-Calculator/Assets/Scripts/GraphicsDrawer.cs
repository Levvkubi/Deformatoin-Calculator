using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsDrawer  : MonoBehaviour
{

    [SerializeField] private bool diffEdges;

    [Space]
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject edgePrefab;

    private DeformationCalculator DC;
    private AKTGenerator AKTG;

    private void Awake()
    {
        DC = GetComponent<DeformationCalculator>();
        AKTG = GetComponent<AKTGenerator>();
    }
    void Start()
    {
        
    }

    public void Drow()
    {
        if (diffEdges)
            drawWithEdges();
        else
            draw();
    }
    private void draw()
    {
        for (int i = 0; i < DC.npq; i++)
        {
            createPoint(pointPrefab,
                        new Vector3(DC.AKT[0, i], DC.AKT[1, i], DC.AKT[2, i]),
                        Quaternion.identity,
                        $"Point {i + 1}");
        }
    }

    private void drawWithEdges()
    {
        foreach (var i in AKTG.verticesIndx)
        {
            createPoint(pointPrefab,
                       new Vector3(DC.AKT[0, i], -DC.AKT[1, i], DC.AKT[2, i]),
                       Quaternion.identity,
                       $"Vertex {i + 1}");
        }

        for (int i = 0; i < AKTG.edgesIndx.Count; i++)
        {
            Quaternion rotation;
            float edgeLenht;
            switch (AKTG.edgesDir[i])
            {
                case 1:
                    rotation = Quaternion.LookRotation(Vector3.right);
                    edgeLenht = AKTG.lx;

                    break;
                case 2:
                    rotation = Quaternion.LookRotation(Vector3.up);
                    edgeLenht = AKTG.ly;
                    break;
                case 3:
                    rotation = Quaternion.LookRotation(Vector3.forward);
                    edgeLenht = AKTG.lz;
                    break;
                default:
                    rotation = Quaternion.identity;
                    edgeLenht = 1;
                    break;
            }

            var edge = createPoint(edgePrefab,
                       new Vector3(DC.AKT[0, AKTG.edgesIndx[i]], -DC.AKT[1, AKTG.edgesIndx[i]], DC.AKT[2, AKTG.edgesIndx[i]]),
                       rotation,
                       $"Edge {AKTG.edgesIndx[i] + 1}");

            if (AKTG.useCustomProportins)
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
