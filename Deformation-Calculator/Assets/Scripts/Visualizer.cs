using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{

    [SerializeField] private bool diffEdges;

    [Space]
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject edgePrefab;

    private DeformationCalculator DC;
    private AKTCreator AKTC;

    private void Awake()
    {
        DC = GetComponent<DeformationCalculator>();
        AKTC = GetComponent<AKTCreator>();
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
        foreach (var i in AKTC.verticesIndx)
        {
            createPoint(pointPrefab,
                       new Vector3(DC.AKT[0, i], -DC.AKT[1, i], DC.AKT[2, i]),
                       Quaternion.identity,
                       $"Vertex {i + 1}");
        }

        for (int i = 0; i < AKTC.edgesIndx.Count; i++)
        {
            Quaternion rotation;
            float edgeLenht;
            switch (AKTC.edgesDir[i])
            {
                case 1:
                    rotation = Quaternion.LookRotation(Vector3.right);
                    edgeLenht = AKTC.lx;

                    break;
                case 2:
                    rotation = Quaternion.LookRotation(Vector3.up);
                    edgeLenht = AKTC.ly;
                    break;
                case 3:
                    rotation = Quaternion.LookRotation(Vector3.forward);
                    edgeLenht = AKTC.lz;
                    break;
                default:
                    rotation = Quaternion.identity;
                    edgeLenht = 1;
                    break;
            }

            var edge = createPoint(edgePrefab,
                       new Vector3(DC.AKT[0, AKTC.edgesIndx[i]], -DC.AKT[1, AKTC.edgesIndx[i]], DC.AKT[2, AKTC.edgesIndx[i]]),
                       rotation,
                       $"Edge {AKTC.edgesIndx[i] + 1}");

            if (AKTC.useCustomProportins)
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
