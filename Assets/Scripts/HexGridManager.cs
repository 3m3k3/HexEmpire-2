using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;  
using System.IO;  
using System. Text; 

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class HexGridManager : MonoBehaviour
{
   
    public MapGenerator mapGenerator = new MapGenerator();
    public HexCell hexCellSource;
    public int rowCount = 11;
    public int columnCount = 20;
    public float x = 0.66f; 
    private int countCell = 0;
    
    public List<HexCell> cells = new List<HexCell>();
    private List<float> mapGrid = new List<float>();
    public Dictionary<int, List<HexCell>> mapCells = new Dictionary<int, List<HexCell>>();
    
    

    private void CreateHexCell(Vector3 position, int row) {
        var cell = Instantiate(hexCellSource, position, Quaternion.identity);
        try {
            cell.init(mapGrid.ElementAt(countCell++), countCell);
             cells.Add(cell);
            if(!mapCells.ContainsKey(row)) {
                mapCells[row] = new List<HexCell>();
           }
            mapCells[row].Add(cell);
        } catch (System.Exception e) {
            Debug.LogError(countCell + " " +mapGrid.Count+ e);
        }      
    }

    public void Build() {
        write(mapGenerator.getTiles(rowCount,columnCount));    
        Clear();
        string[] t= read().Split('#');
        this.mapGrid = t.Select(float.Parse).ToList();
        
        if (hexCellSource == null) {
            Debug.LogError("C'mon you forget to provide the hex cell source");
        } else { 
            Vector3 firstCell = new Vector3(0,0,0);
            
            Vector3 tmp = new Vector3(0,-x,0);
            for (int j = 0; j < rowCount; j++) {                
                if(isPaire(j)) {
                    firstCell = tmp + new Vector3(0,x,0);  
                } else {
                    firstCell = tmp + new Vector3(-x,x,0);  
                }
                for (int i = 0; i < columnCount; i++) {
                    if(i != 0) {
                        firstCell = firstCell + new Vector3(2*x,0,0);
                    }   
                    CreateHexCell(firstCell,j);
                }
                tmp = new Vector3(0,firstCell.y,0);
            }
        }
    }

    public void PopulateCellsFromScene() {
        cells = FindObjectsOfType<HexCell>().ToList();
    }
    

    
    public void Clear() {
        PopulateCellsFromScene();
        foreach (var item in this.cells) {
            DestroyImmediate(item.gameObject);
        }
        this.cells.Clear();
        this.mapGrid.Clear();
        countCell = 0;
    }

    public void CountCells() {
        Debug.LogError(this.cells.Count);
    }
    public void PrintGrid() {
        string s ="";
        foreach (HexCell item in cells)
        {
            s += item.getIndexSprite() + ",";
        }
        write(s.Remove(s.Length-1));
    }

    public void write(string s) {

     string path = @"c:\temp\MyTest.txt";  
  
        // Delete the file if it exists.  
        if (File.Exists(path))  
        {  
            File.Delete(path);  
        }  
  
        //Create the file.  
        using (FileStream fs = File.Create(path))  
        {  
            AddText(fs, s);  
        }  
    }
        public String read() {
        //Open the stream and read it back.
        string path = @"c:\temp\MyTest.txt"; 
        string result = "";
        using (FileStream fs = File.OpenRead(path))  
        {  
            byte[] b = new byte[1024];  
            UTF8Encoding temp = new UTF8Encoding(true);  
            while (fs.Read(b,0,b.Length) > 0)  
            {
                result += temp.GetString(b);  
            }
            Debug.LogError(result);
            return result;  
        }
     }
     private static void AddText(FileStream fs, string value)  
    {  
        byte[] info = new UTF8Encoding(true).GetBytes(value);  
        fs.Write(info, 0, info.Length);  
    }  
    private bool isPaire(int x) {
        return x%2 == 0;
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
            if (GUILayout.Button("Clear")) {
                (target as HexGridManager).Clear();
            }
            if (GUILayout.Button("CountCells")) {
                (target as HexGridManager).CountCells();
            }
            if (GUILayout.Button("Grid")) {
                (target as HexGridManager).PrintGrid();
            }
        }
    }
#endif
}
