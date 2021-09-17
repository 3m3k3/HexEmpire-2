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
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;

    public int seed;

    public bool autoUpdate;



    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        
    }

    

    


    MapData GenerateMapData(Vector2 center)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, center, normalizeMode);

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

 
    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
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