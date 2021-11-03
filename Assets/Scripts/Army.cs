using UnityEngine;
public class Army : MonoBehaviour {
    
    public SpriteRenderer spriteRenderer;
    public IHexCell Cell;
    public Player Player;
    public Vector3 Target;
    public bool OnTheMove;
    public float speed = 100f;
    public bool Played;
    
    int soldier;
    int morale;
    bool diplayStat = false;


    public void init(IHexCell cell, int xMorale, int xSoldier, Player player) {
        Cell = cell;
        Player = player;
        if(Player == null) {
            Debug.LogError("player is null");
        }
        if(Cell != null) {
            Cell.ownIt(this);            
        } else {
            Debug.LogError("cell is null ");
        }
        transform.position = cell.transform.position + new Vector3(0,0.4f,0);
        soldier = xSoldier;
        morale = xMorale;
    }
    public void mixArmies(Army otherArmy) {
        Played = true;
        Target = otherArmy.Cell.transform.position + new Vector3(0f,0.4f,0f);
        OnTheMove = true;
        Cell.army = null;
        Cell = otherArmy.Cell;
        Cell.army = this;
        soldier = soldier + otherArmy.soldier;
        morale = (morale + otherArmy.morale)/2 > soldier ? soldier : (morale + otherArmy.morale)/2;
        
        Player.armies.Remove(otherArmy);
        DestroyImmediate(otherArmy.gameObject);
    }

    

    public void attack(Army otherArmy) {
        Played = true;
        Target = otherArmy.Cell.transform.position + new Vector3(0f,0.4f,0f);
        OnTheMove = true;
        Cell.leaveCell();
        if(otherArmy.getPowerArmy() < getPowerArmy()) { // attack wins   
            Cell = otherArmy.Cell;
            Cell.army = this;
            soldier = soldier - otherArmy.soldier;
            if (soldier<=0) {
                soldier = 1;
            }
            morale = (morale + 20) > soldier ? soldier : morale +20;
            otherArmy.Player.armies.Remove(otherArmy);
            DestroyImmediate(otherArmy.gameObject);
            if(Cell.isCity()) {
            Player.cities.Add(Cell);
            }
        } else {    // attack loose
            otherArmy.soldier = otherArmy.soldier - soldier;
            if(otherArmy.soldier <= 0) {
                otherArmy.soldier =1;
            }
            otherArmy.morale = otherArmy.morale + 20 > otherArmy.soldier ? otherArmy.soldier : otherArmy.morale + 20; 
            Player.armies.Remove(this);           
            DestroyImmediate(this.gameObject);
        }
    }

    public void addTroups(int x) {
        soldier+=x;
        morale = morale + x/2; 
    }
    private int getPowerArmy() {
        return soldier + morale;
    }
    public string getStat() {
        return soldier+"/"+morale;
    }
   
    public string getPos() {
        return gameObject.transform.position +"";
    }
    public void Move(IHexCell targetCell) {        
        Played = true;
        OnTheMove = true;
        Target = targetCell.transform.position + new Vector3(0f,0.4f,0f);
        Cell.army = null;
        Cell = targetCell;
        targetCell.army = this;
        if(Cell.isCity()) {
            Player.cities.Add(Cell);
        }
        
    }

    public string getInfos() {
        string res = "";
        res += soldier + " " + morale;
        return res;
    }

     void Awake(){
        speed = 10f;
        OnTheMove = false;
    }
        
    void Update() {
        if(OnTheMove) {
            float step =  speed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, Target, step);
            if (Vector3.Distance(transform.position, Target) < 0.001f) {   
                OnTheMove = false;
            }
        }        
    }
}