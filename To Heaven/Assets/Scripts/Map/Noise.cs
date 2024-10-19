using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
    public static float[,] GeneratorNoise(int mapWith,int mapHeight,int seed,float scale, int octaves, float persistance,float lacunarity,Vector2 offset){
        float[,] noiseMap = new float[mapWith,mapHeight];
        
        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];
        for(int i = 0 ; i < octaves ;i++){
            float offSetX = prng.Next(-100000,100000) + offset.x ;
            float offSetY = prng.Next(-100000,100000) + offset.y;
            octavesOffsets[i] = new Vector2(offSetX,offSetY);
        }

        if(scale <= 0 ){
            scale = 0.0001f;
        }

        float maxNoiseHeight  = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float haftWidth = mapWith/2f;
        float haftHeight = mapHeight/2f;

        for(int i = 0 ; i < mapHeight ; i++){
            for(int j = 0 ; j < mapWith ; j++){
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int x = 0 ; x < octaves ; x++){
                    float sampleX = (j - haftWidth) / scale*frequency + octavesOffsets[x].x;
                    float sampleY = (i - haftHeight) / scale*frequency + octavesOffsets[x].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX,sampleY) *2 -1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if(noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                }else if(noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[j,i] = noiseHeight;
                }
        }
        for(int y = 0 ; y < mapHeight ; y++){
            for(int x = 0 ; x < mapWith ; x++){
                noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight,maxNoiseHeight,noiseMap[x,y]);
            }
        }
        return noiseMap;
    }
}
