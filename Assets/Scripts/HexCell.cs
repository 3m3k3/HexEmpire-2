using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
     private bool isPort = false;
    private bool isCity = false;
    private bool isCapital = false;
    private bool isSea = false;
    private bool isGrass = false;

    public Nature nature;
    public enum Nature
    {
        Capital,
        City,
        Port,
        Sea,
        Sand
    } 
    public int id;
    public void init (int nature, int id) {
       Test();
        switch (nature)
        {
            case 0 : 
                setCapital();
                break;
            case 1 : 
                setCity();
                break;
            case 2 : 
                setPort();
                break;
            case 3 : 
                setSea();
                break;
            case 4 : 
                setGrass();    
                break;
            case 5 : 
                setDeepWater();    
                break;    
            default: 
                Debug.LogError("Nature cell incorrect");
                break;
        }
        
        this.id = id;

    }
    
   

    private void setCapital() { // 0
        this.isCapital = true;
        this.nature = Nature.Capital;
        ChangeSprite(0);
    }


    private void setDeepWater() {
        ChangeSprite(5);
    }  
    

    private void setCity() { // 1
        this.isCity = true;
        this.nature = Nature.City;
        ChangeSprite(1);
    }
    private void setPort() { // 2 
        this.isPort = true;
        this.nature = Nature.Port;
        ChangeSprite(2);
    }
    private void setSea() { // 3
        this.isSea= true;
        this.nature = Nature.Sea;
        ChangeSprite(3);
    }

    private void setGrass() { // 4
        this.isGrass = true;
        this.nature = Nature.Sand;
        ChangeSprite(4);
    }

    public void OnMouseDown() {
        Debug.LogError(id+" : "+nature);
    }

    public bool IsCity() {
        return this.isCity;
    }

    void ChangeSprite(int index)
        {
            spriteRenderer.sprite = spriteArray[index]; 
        }

    public int getIndexSprite() {
        int i = 0;
        Debug.Log(spriteRenderer.sprite + " " +id);
        while (spriteRenderer.sprite != spriteArray[i] && i < spriteArray.Length)
        {
             i++;
        }
        return i;
    }

    public void Test() {
        GameObject objToSpawn  = new GameObject("Cool GameObject made from Code");
        objToSpawn.transform.parent = this.transform;
     }
    
}
