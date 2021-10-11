using UnityEngine;

public class BeginTurnState : State {

   
    

    public override void OnStateEnter(int idCell) {
        Player playerCell = GameWorld.getCell(idCell).owner;
        if (playerCell == null) {
            Debug.LogError("playerCell null");
        } else {
        }
        if(playerCell == GameWorld.currentPlayer) {
            GameWorld.SetState(new SelectTroopsState(), idCell);
            

        }
    }
    public override void OnStateExit() { 
    }
        

   
}