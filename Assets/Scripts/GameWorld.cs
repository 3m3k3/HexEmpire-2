using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameWorld : MonoBehaviour
{
    public List<Player> players;
    List<IHexCell> cells;
    public Player currentPlayer; 
    public Player PlayerSource; 

    public State State;

    public Sprite mySprite; 

    
    public HexGridManager hexGridManager; 
    // Start is called before the first frame update
    void Start()
    {   
        hexGridManager.world = this;
        hexGridManager.Build();
        cells = hexGridManager.cells;
        Player p1 = Instantiate(PlayerSource);
        Player p2 = Instantiate(PlayerSource);
        p1.init(cells[0],cells[0].transform.position, "blue", Color.blue);
        p2.init(cells[19],cells[19].transform.position, "red", Color.red);
        currentPlayer = p1;
        State = new BeginTurnState();
        State.init(this);
        Debug.Log("World init done");
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cellClik(int id) {
        State.OnStateEnter(id);
    }

    public IHexCell getCell(int idCell) {
        return cells.ElementAt(idCell);
    }

    public HashSet<int> HighLightNeighbours(int idCell) {
        HashSet<int> tmpSet = new HashSet<int>(getCell(idCell).neighbours);
        HashSet<int> set = new HashSet<int>();
        IHexCell tmp = null;
        foreach (var item in tmpSet)
        {
            tmp = getCell(item);
            set.UnionWith(new HashSet<int>(tmp.neighbours));
        }
        set.Remove(idCell);
       
        foreach (var item in set)
        {
            tmp = getCell(item);
            // Debug.LogError("HighLight : " + item);
            tmp.GetComponent<Renderer>().material.color = currentPlayer.Color;
        } 
        return set;
    }

     public void undoHighLightNeighbours(HashSet<int> set) {
        IHexCell tmp = null;
        foreach (var item in set)
        {
            tmp = getCell(item);
            tmp.GetComponent<Renderer>().material.color = Color.white;

        }
    }

     public void SetState(State state, int idCell) {
        if (State != null)
           
            State.OnStateExit();
        State = state;
        
        if (State != null) {
            State.init(this);
            State.OnStateEnter(idCell);
        }
    }

}
