using System.Collections.Generic;
using UnityEngine;

public class PointsCreator : MonoBehaviour
{
    [SerializeField] private int nx;
    [SerializeField] private int ny;
    [SerializeField] private int nz;

    [SerializeField] private int distanceBetweenPoints = 1;

    [SerializeField] private List<GameObject> vertices;
    [SerializeField] private List<GameObject> edges;

    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject edgePrefab;

    void Start()
    {
        vertices = new List<GameObject>();

        createPoints();
    }

    private void createPoints()
    {
        List<KeyValuePair<int, Vector3>> prepEdge = new List<KeyValuePair<int, Vector3>>();
        prepEdge.Add(new KeyValuePair<int, Vector3>(nx, Vector3.right * distanceBetweenPoints / 2f));
        prepEdge.Add(new KeyValuePair<int, Vector3>(ny, Vector3.up * distanceBetweenPoints / 2f));
        prepEdge.Add(new KeyValuePair<int, Vector3>(nz, Vector3.forward * distanceBetweenPoints / 2f));

        for (int x = 0; x <= nx; x++)
        {
            for (int y = 0; y <= ny; y++)
            {
                for (int z = 0; z <= nz; z++)
                {
                    var position = new Vector3(
                                x * distanceBetweenPoints,
                                y * distanceBetweenPoints,
                                z * distanceBetweenPoints);

                    vertices.Add(
                        Instantiate(
                            pointPrefab,
                            position,
                            Quaternion.identity));

                    int[] indexes = { x, y, z };

                    for (int i = 0; i < 3; i++)
                        if (indexes[i] < prepEdge[i].Key)
                            edges.Add(
                                Instantiate(
                                    edgePrefab,
                                    position + prepEdge[i].Value,
                                    Quaternion.LookRotation(prepEdge[i].Value)));
                }
            }
        }
    }
}
