using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.IO;  
using System. Text; 

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class HexGridManager : MonoBehaviour
{
   
  
    public MapGenerator mapGenerator;
    
    public HexSea_0 hexSeaSource;

    public HexSea_1 hexDeepSeaSource;

    public HexSand_1 hexHighSandSource;    
    
    public HexSand_0 hexSandSource;

    public HexCity hexCitySource;

    public HexPort hexPortSource;

    public GameWorld world;

    
    private const int DEEP_WATER_LEVEL = 450;
    private const int WATER_LEVEL = 1500;
    private const int SAND_LEVEL = 5000;
    private const int HIGH_SAND_LEVEL = 10000;
    

    public int y = 11;
    public int x = 20;
    public int seed;
    public int altitude = 80; 
    public float ecartTuile = 0.66f; 
    private int countCell = -1;
    
    public List<IHexCell> cells = new List<IHexCell>();
    private List<int> mapGrid = new List<int>();
    public Dictionary<int, List<IHexCell>> mapCells = new Dictionary<int, List<IHexCell>>();
    
    

    private void CreateHexCell(Vector3 position, int row) {
         
        int intNature = mapGrid.ElementAt(countCell);
        IHexCell cell = createAndReturnXexCellInstance(intNature, position);
        cell.transform.parent = gameObject.transform;
        cell.init(countCell, world,x,y);
        cells.Add(cell);
        countCell++;
        if(!mapCells.ContainsKey(row)) {
            mapCells[row] = new List<IHexCell>();
        }
        mapCells[row].Add(cell);
            
    }

    public IHexCell createAndReturnXexCellInstance(int tileNature, Vector3 position) {
        if( tileNature ==  ((int)Nature.City) ) {
            return Instantiate(hexCitySource, position, Quaternion.identity);
        } 
        if( tileNature ==  ((int)Nature.Port) ) {
            return Instantiate(hexPortSource, position, Quaternion.identity);
        }
        //Debug.Log(tileNature);
        if(tileNature < DEEP_WATER_LEVEL) {
            return Instantiate(hexDeepSeaSource, position, Quaternion.identity);
        } else if (tileNature < WATER_LEVEL) {
            return Instantiate(hexSeaSource, position, Quaternion.identity);
        } else if (tileNature < SAND_LEVEL) {
            return Instantiate(hexSandSource, position, Quaternion.identity);
        } else {
            return Instantiate(hexHighSandSource, position, Quaternion.identity);
        }
    }


    public void Build() {
        Clear();
        mapGrid = mapGenerator.getRandomMap(x,y,seed, altitude);
        if (hexSeaSource == null || hexSandSource == null) {
            Debug.LogError("C'mon you forget to provide the hex cell source");
        } else { 
            Vector3 firstCell = new Vector3(0,0,0);
            Vector3 tmp = new Vector3(0,-ecartTuile,0);
            for (int j = 0; j < y; j++) {                
                if(isPaire(j)) {
                    firstCell = tmp + new Vector3(0,ecartTuile,0);  
                } else {
                    firstCell = tmp + new Vector3(-ecartTuile,ecartTuile,0);  
                }
                for (int i = 0; i < x; i++) {
                    if(i != 0) {
                        firstCell = firstCell + new Vector3(2*ecartTuile,0,0);
                    }   
                    CreateHexCell(firstCell,j);
                }
                tmp = new Vector3(0,firstCell.y,0);
            }
         // updateMapGrid();
        }
    }

    public void getTiles() {
        mapGenerator.getRandomMap(x,y,seed, altitude);
    }
    //Function to get a random number 
    private static readonly System.Random random = new System.Random(); 
    private static readonly object syncLock = new object(); 
    public static int RandomNumber(int min, int max)
    {
        lock(syncLock) { // synchronize
            return random.Next(min, max);
        }
    }



    public void PopulateCellsFromScene() {
        cells = FindObjectsOfType<MonoBehaviour>().OfType<IHexCell>().ToList();
    }
    
    private static readonly ThreadLocal<System.Random> appRandom
     = new ThreadLocal<System.Random>(() => new System.Random());
    
    public void Clear() {
        PopulateCellsFromScene();
        foreach (var item in this.cells) {
            item.destroy();
        }
        this.cells.Clear();
        this.mapGrid.Clear();
        countCell = 0;
    }

    public void CountCells() {
        Debug.LogError(this.mapGrid.Count);
    }

    private bool isPaire(int x) {
        return x%2 == 0;
    }

    public void RandomBuild() {
        System.Random rnd = new System.Random();
        seed = rnd.Next(0, 1000);  // creates a number between 1 and 12
        Build();
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(HexGridManager))]
    class MyEditor : Editor {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Build")) {
                (target as HexGridManager).Build();
            }
            if (GUILayout.Button("Random Build")) {
                (target as HexGridManager).RandomBuild();
            }
            if (GUILayout.Button("Clear")) {
                (target as HexGridManager).Clear();
            }
            if (GUILayout.Button("CountCells")) {
                (target as HexGridManager).CountCells();
            }
            if (GUILayout.Button("tiles")) {
                (target as HexGridManager).getTiles();
            }
            
        }
    }
#endif
}
