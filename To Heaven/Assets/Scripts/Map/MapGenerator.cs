using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWith;
    public int mapHeight;
    public float scale;
    public void GeneratorMap(){
        float [,] noiseMap = Noise.GeneratorNoise(mapWith,mapHeight,scale);

        MapDisPlay disPlay = FindAnyObjectByType<MapDisPlay>();
        disPlay.DrawNoiseMap(noiseMap);

    }

}
