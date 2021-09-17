using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{


    public Noise.NormalizeMode normalizeMode;

    public int mapHeight = 20;
    public int mapWidth = 10;
    [Range(0,10)]
    private float noiseScale = 7.6f;

    private int octaves = 50;
    private float persistance = 0.57f;

    public int seed;

    public bool autoUpdate;


    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData();
        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        
    }

    public String getTiles(int mapWidth, int mapHeight) {
        string result = "";
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, normalizeMode);
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                
                result += noiseMap[x, y] + "#";
            }
        }
        return result.Remove(result.Length-1);
    }
    MapData GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, normalizeMode);

        Color[] colourMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                
                float currentHeight = noiseMap[x, y];
                Debug.Log(currentHeight);
            }
        }
        return new MapData(noiseMap, colourMap);
    }
}

 


public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;

    public MapData (float[,] heightMap, Color[] colourMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}