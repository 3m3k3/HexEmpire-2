using UnityEngine;

public class BeginTurnState : State {

   
    public override void OnStateEnter(int idCell) {
        if(idCell >= 0) {
            Army armyCell = GameWorld.getCell(idCell).army;
            if (armyCell == null) {
                Debug.LogError("armyCell null");
            } else if(armyCell.Player == GameWorld.GetCurrentPlayer() && !armyCell.Played) {
                GameWorld.SetState(new SelectTroopsState(), idCell);        
            }            
        }
    }
    public override void OnStateExit() { 
    }
        

   
}