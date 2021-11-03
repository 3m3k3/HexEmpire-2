using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class SelectTroopsState : State {

    private HashSet<int> highLightCells;
    private bool highLight = false;

    private int previousTarget;
    public override void OnStateEnter(int idCell) {
        
        if(highLight) {
           // Debug.Log("cell already lighted");           
            if(highLightCells.Contains(idCell)) {  
                Debug.Log("contains " +idCell);              
                State moveState = new MoveState();
                ((MoveState)moveState).previousTarget = previousTarget;
                GameWorld.SetState( moveState, idCell);
            } else {
                GameWorld.SetState(new BeginTurnState(), idCell);
            }
        } else {
        //    Debug.Log("Highlighting cell "+ idCell);
            highLightCells = new HashSet<int>();
            previousTarget = idCell;
            highLightCells = GameWorld.HighLightNeighbours(idCell);
            highLight = true;
        }
        
    }
    public override void OnStateExit() {
        GameWorld.undoHighLightNeighbours(highLightCells);
        highLightCells.Clear();
    }
        

   
}