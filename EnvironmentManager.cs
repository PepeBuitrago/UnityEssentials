using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;

    [SerializeField]
    private int cantTrees, probabilityTree, probabilityRock, probabilityLake;

    [SerializeField]
    private GameObject[] treesPrefab;

    [SerializeField]
    private GameObject[] rocksPrefab;

    [SerializeField]
    private GameObject rainFx, fogFx;

    [SerializeField]
    private bool raining;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (SaveSystem.FileExists("Terrain"))
        {
        }
        else
        {
            
        }
    }

    private void Update()
    {
        if (!raining)
        {
            RandomRain();
        }
    }

    public static List<GameObject> GenerateChunkResources(Mesh meshTerrain, GameObject parentObject)
    {
        int skipAmount = 10; // Cantidad de vértices para saltar en cada iteración
        Vector3[] vertices = meshTerrain.vertices; // Obtén los vértices de la malla

        List<GameObject> treeList = new List<GameObject>();


        treeList.Clear();

        for (int i = 0; i < vertices.Length; i += skipAmount + 1)
        {
            Vector3 currentVertex = vertices[i]; // Accede al vértice actual

            if (Random.Range(0, 2) == 1)
            {
                if(currentVertex.y > 0)
                {
                    if (Random.Range(1, 30) == 1)
                    {
                        // Haz algo con el vértice actual, por ejemplo, imprímelo
                        GameObject newTree = Instantiate(Instance.rocksPrefab[Random.Range(0, Instance.rocksPrefab.Length)], parentObject.transform.position + currentVertex, Quaternion.identity, parentObject.transform);
                        treeList.Add(newTree);
                    }
                }
            }
            else
            {
                if (currentVertex.y > 3)
                {
                    if (Random.Range(1, 10) == 1)
                    {
                        if(Random.Range(1, 100) == 1)
                        {
                            // Haz algo con el vértice actual, por ejemplo, imprímelo
                            GameObject newTree = Instantiate(Instance.treesPrefab[Random.Range(14, Instance.treesPrefab.Length)], parentObject.transform.position + currentVertex, Quaternion.identity, parentObject.transform);
                            treeList.Add(newTree);
                        }
                        else
                        {
                            // Haz algo con el vértice actual, por ejemplo, imprímelo
                            GameObject newTree = Instantiate(Instance.treesPrefab[Random.Range(0, 15)], parentObject.transform.position + currentVertex, Quaternion.identity, parentObject.transform);
                            treeList.Add(newTree);
                        }
                    }
                }
            }
        }

        return treeList;
    }

    public void RandomRain()
    {
        if(Random.Range(0,100000) == 1)
        {
            raining = true;
            rainFx.SetActive(raining);
            StartCoroutine(ResetRain(Random.Range(0, 60)));
        }
    }

    IEnumerator ResetRain(float delay)
    {
        yield return new WaitForSeconds(delay);
        raining = false;
        rainFx.SetActive(raining);
    }
}
