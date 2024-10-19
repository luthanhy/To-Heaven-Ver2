using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
    public static float[,] GeneratorNoise(int mapWith,int mapHeight,float scale){
        float[,] noiseMap = new float[mapWith,mapHeight];
        if(scale <= 0 ){
            scale = 0.0001f;
        }
        for(int i = 0 ; i < mapHeight ; i++){
            for(int j = 0 ; j < mapWith ; j++){
                float sampleX = j / scale;
                float sampleY = i / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX,sampleY);
                noiseMap[j,i] = perlinValue;
            }
        }
        return noiseMap;
    }
}
