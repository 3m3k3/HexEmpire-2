using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Player : MonoBehaviour {


    float morale;
    public List<Army> armies;

    public List<IHexCell> cities;

    public string Name;
   
    public Army ArmySource;
    public Color Color;

    public Army init(IHexCell cell, string name, Color color) {
        Vector3 position = cell.transform.position;
        Color = color;
        Army capitale = createArmy(cell);
        armies.Add(capitale);
        cities.Add(cell);
        Name = name;
        return capitale;
    }

    private Army createArmy(IHexCell cell) {
        Vector3 position = cell.transform.position;
        Army a = Instantiate(ArmySource, position + new Vector3(0,0.4f,0), Quaternion.identity);
        a.init(cell, 10,10,this);
        a.spriteRenderer.color = Color;
        return a;
    }

    
    public int getNumberOfArmyReadyToBattle() {
        return armies.FindAll(x => !x.Played).Count();
    }
    public void looseArmy(Army a) {
           armies.Remove(a);
           IHexCell cell = a.Cell;
           cell.ownIt(null); 
           DestroyImmediate(a.gameObject);
    }

    public void ReEngageTroopsAndReMobiliseArmy() {
        cities.ForEach(x => reEngageCity(x));
        armies.ForEach(x => x.Played = false);        
    }

    private void reEngageCity(IHexCell cell) {
        Debug.Log(cell.id + " reengage");
        if(cell.army == null) {
            Army city = createArmy(cell);
            armies.Add(city);
            armies.ForEach(x => Debug.Log(x.Cell.id + " zae"));
        } else {
            Debug.Log(cell.id + " cell army not null");
            cell.army.addTroups(10);
        }

    }
   
}