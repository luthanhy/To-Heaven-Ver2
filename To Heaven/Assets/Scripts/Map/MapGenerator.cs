using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWith;
    public int mapHeight;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public void GeneratorMap(){
        float [,] noiseMap = Noise.GeneratorNoise(mapWith,mapHeight,seed,scale,octaves,persistance,lacunarity,offset);

        MapDisPlay disPlay = FindAnyObjectByType<MapDisPlay>();
        disPlay.DrawNoiseMap(noiseMap);


    }

}
