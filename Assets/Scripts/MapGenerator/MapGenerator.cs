using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
public class MapGenerator : MonoBehaviour
{


    public Noise.NormalizeMode normalizeMode;
    /*
        Terrain = ensemble de tuiles terrain (eau, terre, sable, ..)
        Map = 
    */
    public List<int> getRandomMap(int x, int y, int seed, int altitude) {
        List<int> indiceListTerrain = new List<int>();
        float[,] noiseMap = Noise.GenerateNoiseMap(x, y, seed, normalizeMode);
        for (int yy = 0; yy < y; yy++)
        {
            for (int xx = 0; xx < x; xx++)
            {
                int indice = (int) (100*Math.Round(noiseMap[xx, yy], 2));
               // Debug.Log(indice * indice + altitude + " ss");
                indiceListTerrain.Add(indice * indice + altitude);
            }
        }
        indiceListTerrain = addCitiesIndice(indiceListTerrain,x,y);
        indiceListTerrain = addPortsIndice(indiceListTerrain,x,y);

        return indiceListTerrain;
    }


    public List<int> addPortsIndice(List<int> indiceListTerrain,  int x, int y) { 
        int cities = 10;
        bool found = false;
        int numberTry = 0;
        List<int> citiesList = new List<int>();
        int randomCell = -1;
        for (int i = 0; i < cities; i++) {
            numberTry = 0;
            while(!found && numberTry < 100) {
                numberTry++;
                randomCell = RandomNumber(0, x*y);
                found = !citiesList.Contains(randomCell) && isPortable(randomCell, indiceListTerrain, x, y);
            }
            if(found) {
                indiceListTerrain[randomCell] = ((int)Nature.Port);
                //Debug.LogError("Ports posed : " +randomCell);
                citiesList.Add(randomCell);
            }
            found = false;
        }
        return indiceListTerrain;
    }
    public List<int> addCitiesIndice(List<int> indiceListTerrain,  int x, int y) {
        int cities = (x*y)/5;
        bool found = false;
        int numberTry = 0;
        List<int> citiesList = new List<int>();
        int randomCell = -1;
        for (int i = 0; i < cities; i++) {
            numberTry = 0;
            while(!found && numberTry < 100) {
                numberTry++;
                randomCell = RandomNumber(0, x*y);
                found = !citiesList.Contains(randomCell) && isUrbanisable(randomCell, indiceListTerrain, x, y);
            }
            if(found) {
                indiceListTerrain[randomCell] = ((int)Nature.City);
                // Debug.LogError("City posed : " +randomCell);
                citiesList.Add(randomCell);
            }
            found = false;
        }
        return indiceListTerrain;
    }


    private bool isPortable(int id, List<int> indiceListTerrain,  int x, int y) { 
        if(isLand(indiceListTerrain[id])) {
            // Debug.Log("dq");
            HashSet<int> set = getNeighbours(id, indiceListTerrain, x, y);
            string s ="";
            foreach (var item in set)
            {
                s+= " " +item;
                if(isSea(indiceListTerrain[item])) { 
                    // Debug.LogError("False " + id + " : ["+s+"]");
                    return true;
                }
            }
            // Debug.LogError("True "+ id + " : ["+s+"]");
            return false;
        } 
        return false;
    }
    private bool isUrbanisable(int id, List<int> indiceListTerrain,  int x, int y) {
        if(isLand(indiceListTerrain[id])) {
            // Debug.Log("dq");
            HashSet<int> set = getNeighbours(id, indiceListTerrain, x, y);
            string s ="";
            foreach (var item in set)
            {
                s+= " " +item;
                if(!isLand(indiceListTerrain[item])) { 
                    // Debug.LogError("False " + id + " : ["+s+"]");
                    return false;
                    }
            }
            // Debug.LogError("True "+ id + " : ["+s+"]");
            return true;
        } 
        return false;
        
    }
    private bool isCity(int id) {
        Nature n = DictionnaryIndiceToNature.getNature(id);
        
        switch (n)
        {
            case Nature.City : 
                return true;
            case Nature.Capital : 
                return true;
            default : 
                return false;
        }
    }
    private bool isLand(int id) {
        Nature n = DictionnaryIndiceToNature.getNature(id);
        switch (n)
        {
            case Nature.Sand_0 : 
                return true;
            case Nature.Sand_1 : 
                return true;    
            default : 
                return false;
        }
        
    }

    private bool isSea(int id) {
        Nature n = DictionnaryIndiceToNature.getNature(id);
        switch (n)
        {
            case Nature.Sea_0 : 
                return true;
            case Nature.Sea_1 : 
                return true;    
            default : 
                return false;
        }
    }


    private HashSet<int> getNeighbours(int id, List<int> indiceListTerrain,  int x, int y) {
        HashSet<int> neighbours;
        int[] valuesI = {             
                            id - 1, 
                            x + id - 1, 
                            x + id, 
                            id + 1,  
                            id - x,  
                            id - x - 1 
                        };
         int[] valuesP = {             
                            id - 1, 
                            x + id, 
                            x + id + 1,
                            id + 1, 
                            id - x + 1,
                            id - x
                        };
        int[] values;  
        int pX = id%x;
        int pY = id%y;

        int t = id - (id%x);
        int count = 0;

        while (t >= x) {
            t =  t - x;
            count++;
        }
        if((count)%2 == 0) {
            values = valuesP;
            if  ( pX == x - 1) {
                values[3] = -1;
                values[4] = -1;
                values[2] = -1;
            } else if (pX == 0) {
                values[0] = -1;
            }
        } else {
            values = valuesI;
            if ( pX == x - 1) {
                values[3] = -1;
            } else if (pX == 0) {
                values[0] = -1;
                values[1] = -1;
                values[5] = -1;
            }
        }
        neighbours = new HashSet<int>(values.Where(i => (i >= 0) && (i < x*y)));
        /*
        string s = "";
        foreach (var item in neighbours)
            {
                s+= " " +item;
            }
            Debug.LogError("neigburgh of "+id+": ["+s+"]");
        */
        return neighbours;
    }

    //Function to get a random number 
    private static readonly System.Random random = new System.Random(); 
    private static readonly object syncLock = new object(); 
    public static int RandomNumber(int min, int max) {
        lock(syncLock) { // synchronize
            return random.Next(min, max);
        }
    }

        
}