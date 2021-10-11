using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {


    float morale;
    public List<Army> armies;

    public string Name;
   
    public Army ArmySource;
    public Color Color;
    private void Start() {
    }    
    public Army init(IHexCell cell, Vector3 position, string name, Color color) {
        Color = color;
        Army capitale = Instantiate(ArmySource, position + new Vector3(0,0.4f,0), Quaternion.identity);
        capitale.init(cell, 10,10,this);
        capitale.spriteRenderer.color = Color;
        armies.Add(capitale);
        Name = name;
        return capitale;
    }

    public Army createArmy(IHexCell cell) {
        Army army = Instantiate(ArmySource, cell.transform.position + new Vector3(0,0.4f,0), Quaternion.identity);
        armies.Add(army);
        army.init(cell, 10,10,this);
        Debug.Log("Im up");
        return army;
    }

}