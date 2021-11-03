using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameWorld : MonoBehaviour
{
    public List<Player> players;
    List<IHexCell> cells;
    int currentPlayer = 0;

    Canvas canvas;
    public bool displayArmyScore = false;
    Text stat;
    public int turn = 0;

    int currentMove = 1; 
    public Player PlayerSource; 

    public State State;

    public Sprite mySprite; 

    public RectTransform m_parent;
    public Camera m_uicamera;

    Text movesText;
    Button endTurnButton;
    private void Awake() {
        canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        movesText = canvas.GetComponentsInChildren<Text>().ToList().Find(x => x.name == "Moves");
        endTurnButton = canvas.GetComponentsInChildren<Button>().ToList().Find(x => x.name == "endTrun");
        
    }
    
    public HexGridManager hexGridManager; 
    // Start is called before the first frame update
    void Start()
    {   
        hexGridManager.world = this;
        hexGridManager.Build();
        cells = hexGridManager.cells;
        List<IHexCell> cities = cells.FindAll(x => x.isCity());
        
        Player p1 = Instantiate(PlayerSource);
        Player p2 = Instantiate(PlayerSource);
        p1.init(cities[0], "blue", Color.blue);
        p2.init(cities[cities.Count()-1], "red", Color.red);
        players.Add(p1);
        players.Add(p2);
        currentPlayer = 0;
        currentMove = 1;
        refreshMoveUIText();
        State = new BeginTurnState();
        State.init(this);
        Debug.Log("World init done");
    }

    public Player GetCurrentPlayer() {
        return players[currentPlayer];
    }
    public void PlayerMoove() {
        currentMove--;        
        int numberOfArmyReadyToBattle = players[currentPlayer].getNumberOfArmyReadyToBattle();
        currentMove = numberOfArmyReadyToBattle < currentMove ? numberOfArmyReadyToBattle : currentMove;
        refreshMoveUIText();
        Debug.Log("currentMove :" + currentMove + " numberOfArmyReadyToBattle : " +numberOfArmyReadyToBattle);
        if(currentMove <= 0) {
            endTurn();
        }
        
    }

    private void refreshMoveUIText() {
        movesText.color = players[currentPlayer].Color;
        movesText.text = "move " + currentMove;
    }

    public void endTurn() {
        Debug.Log("End Turn "+ players[currentPlayer].Color);
        players[currentPlayer].ReEngageTroopsAndReMobiliseArmy();
        currentPlayer = (currentPlayer+1) % players.Count();
        setCurrentMove();
        refreshMoveUIText();        
    }
    
    private void setCurrentMove() {
        if(players[currentPlayer] != null ) {
            int tempMove = players[currentPlayer].armies.Count();
            if(tempMove == 0) {
                players.RemoveAt(currentPlayer);
                endTurn();
            } else {
                currentMove =  tempMove > 5 ? 5 : tempMove; 
            }          
        } else {
            Debug.LogError("FuckiN PROBLEM HERE");
        }
    }
    public void cellClik(int id) {
        State.OnStateEnter(id);
    }

    public void UnDisplayStatArmy() {
        Text yourText = canvas.GetComponentInChildren<Text>();
        yourText.enabled = false;
    }
    public void DisplayStatArmy(int idCell) {
        Text yourText = canvas.GetComponentInChildren<Text>();
        
        Vector2 anchorPos;
        Vector3  mousePos = Input.mousePosition;
        mousePos.z = 1;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, m_uicamera.WorldToScreenPoint(getCell(idCell).transform.position), m_uicamera, out anchorPos);
        RectTransform assign_text_1RT = yourText.GetComponent<RectTransform>();
        assign_text_1RT.anchoredPosition = anchorPos;
        
        if(getCell(idCell).army != null) {
            yourText.text= getCell(idCell).army.getStat();
            
            yourText.enabled = true;
        }
        
    }

    public IHexCell getCell(int idCell) {
        return cells.ElementAt(idCell);
    }

    public HashSet<int> HighLightNeighbours(int idCell) {
        HashSet<int> tmpSet = new HashSet<int>(getCell(idCell).neighbours);
        HashSet<int> set = new HashSet<int>();
        HashSet<int> armies = new HashSet<int>();
        IHexCell tmp = null;
        foreach (var item in tmpSet)
        {
            tmp = getCell(item);
            if(!tmp.isSea()) {
                if (tmp.army != null) {
                    Debug.Log("army here");
                    armies.Add(item);
                } else {
                    set.UnionWith(new HashSet<int>(tmp.neighbours));
                    set.Add(item);
                } 
            }
        }
        set.Remove(idCell);
        HashSet<int> seaSet = new HashSet<int>();
        foreach (var item in set)
        {
            tmp = getCell(item);
            if (tmp.isSea()) {
                seaSet.Add(item);
            }
        }
        set.ExceptWith(seaSet);
        set.UnionWith(new HashSet<int>(armies));
        foreach (var item in set)
        {
            tmp = getCell(item);
            tmp.GetComponent<Renderer>().material.color = players[currentPlayer].Color;
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
