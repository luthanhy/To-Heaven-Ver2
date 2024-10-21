using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode{NoiseMap,ColourMap,Mesh};
    public DrawMode drawMode;
    public int mapWith;
    public int mapHeight;
    public float scale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public TerrainType[] regions;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public void GeneratorMap(){
        float [,] noiseMap = Noise.GeneratorNoise(mapWith,mapHeight,seed,scale,octaves,persistance,lacunarity,offset);
        Color[] colourMap = new Color[mapWith*mapHeight];
        for(int y  = 0 ; y < mapHeight ; y++){
            for(int x = 0 ; x < mapWith ; x++){
                float currentHeight = noiseMap[x,y];
                for(int i = 0 ; i < regions.Length ; i++){
                    if(currentHeight < regions[i].height){
                        colourMap[y*mapWith + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisPlay disPlay = FindAnyObjectByType<MapDisPlay>();
        if(drawMode == DrawMode.NoiseMap){
        disPlay.DrawTexture(TextureGenerator.textureFromHeightMap(noiseMap));
        }else if(drawMode == DrawMode.ColourMap){
            disPlay.DrawTexture(TextureGenerator.textureFromColourMap(colourMap,mapWith,mapHeight));
        }else if(drawMode == DrawMode.Mesh){
            disPlay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap),TextureGenerator.textureFromColourMap(colourMap,mapWith,mapHeight));
        }
    }

}

[System.Serializable]
public struct TerrainType{
    public string name;
    public float height;
    public Color colour;
}