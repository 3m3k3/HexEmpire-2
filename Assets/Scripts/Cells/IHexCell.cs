using UnityEngine;

using System.Collections.Generic;
using System.Linq;

 abstract public class IHexCell : MonoBehaviour {


    private GameWorld World;

    public Army army = null;
    private SpriteRenderer spriteRenderer;
    public int id;
  
    public Player PlayerController = null;
    bool isDisplayArmyScore;

    public HashSet<int> neighbours;
    public void ownIt(Army newArmyOwner) {
        if(newArmyOwner != null) {           
            army = newArmyOwner;
        } else {
            Debug.LogError("army is null " + id);
            army = null;
        }
    }

    private void Awake() {
        army = null;
    }
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void leaveCell() {
        army.Cell = null;
        army = null;

    }

    public void changeSprite(Sprite newSprite) {
        spriteRenderer.sprite = newSprite; 
    }
    private void OnMouseDown() {
        Debug.Log("clik on " +id + " - infos : " + getInfos());
        World.cellClik(id);    
    }

    public string getInfos() {
        string res = "army : ";
        if(army != null) {
            res += army.getInfos();
        } else {
            res += "null";
        }
        return res;
    }

    private void OnMouseOver() {        
        if(army != null) {
            World.DisplayStatArmy(id);
        } else {
               
        }
    }

    private void OnMouseExit() {
        if(army != null) {
            World.UnDisplayStatArmy();
        }
    }
 
    public List<GameObject> objects; 
    public void destroy() {
        DestroyImmediate(gameObject);
    }

    public void init(int intId, GameWorld world, int x, int y) {
         if(world == null) {
            Debug.LogWarning("world is nul");
        }
        this.World = world;
        this.id = intId;
        calculNeighbours(x,y);
        for (int i = 0; i < objects.Count; i++)
        {
            GameObject g = Instantiate(objects[i], this.transform.position+ new Vector3(0,0.4f,0), Quaternion.identity);
            SpriteRenderer sprite = g.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = i+2;            
            g.transform.parent = gameObject.transform;        
        }
    }
    
    
    private void calculNeighbours(int x, int y) {
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
    }   

    public abstract bool isLand();
    public abstract bool isSea();
    public abstract bool isCity();
    public abstract bool isPort();
    public abstract bool isCapital();


}