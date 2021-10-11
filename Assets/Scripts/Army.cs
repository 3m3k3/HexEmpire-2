using UnityEngine;

public class Army : MonoBehaviour {
    
    public SpriteRenderer spriteRenderer;

    int soldier;

    int morale;
    IHexCell Cell;

    public Player Player;
    public Vector3 Target;
    bool hasPlay;

    public bool OnTheMove;
    public void init(IHexCell cell, int xMorale, int xSoldier, Player player) {
        Cell = cell;
        if(Cell != null) {
            Cell.owner = player; 
        }
        
        transform.position = cell.transform.position;
        soldier = xSoldier;
        morale = xMorale;
        Player = player;
    }
    public void mixArmies(Army otherArmy) {
        soldier = soldier + otherArmy.soldier;
        morale = (morale + otherArmy.morale)/2;
        DestroyImmediate(otherArmy);
    }

    public string getPos() {
        return gameObject.transform.position +"";
    }
    public void Move(Vector3 target) {        
        OnTheMove = true;
        Target = target;    
        Debug.Log(target + " "+ this.transform.position + " "  + OnTheMove);
    }
     void Awake(){
        speed = 10f;
        OnTheMove = false;
    }
        
    void Update() 
    {
        Debug.Log(OnTheMove+ " " + Target);

        if(OnTheMove) { 
            Debug.Log("update move "+ OnTheMove + " " + Target +" " + gameObject.transform.position);
            float step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, Target, step);
            
        }
    }
       /*
        if (Vector3.Distance(transform.position, Target) < 0.001f)
            {   
                Debug.Log("no more on the move");
                OnTheMove = false;
            }
            
        */
        
    

     public float speed = 100f;
     


}