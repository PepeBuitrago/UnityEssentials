using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveTerrainDataClass
{
    public Dictionary<Vector2, SaveTerrainChunkDataClass> chunkList = new Dictionary<Vector2, SaveTerrainChunkDataClass>();

    public void AddChunk(SaveTerrainChunkDataClass chunk, Vector2 id)
    {
        chunkList.Add(id, chunk);
    }
}
