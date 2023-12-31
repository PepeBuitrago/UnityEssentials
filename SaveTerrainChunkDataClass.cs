using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveTerrainChunkDataClass
{
    public Vector2 id;
    public Vector3[] vertices; // Datos de los vértices de la malla
    public Vector3[] resourcesCoords;
}
