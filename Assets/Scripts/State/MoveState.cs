using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class MoveState : State {

    public int previousTarget;

    public override void OnStateEnter(int idCell) {        
        IHexCell from =  GameWorld.getCell(previousTarget);     
        IHexCell to =  GameWorld.getCell(idCell);
        
        if(to.army != null) { 
            // Case where there is an enemy or ally army
            Debug.Log("Attock or Mix  : " + to.army);
            if (to.army.Player == from.army.Player) {
                
                Debug.Log("Mix");
                from.army.mixArmies(to.army);
            
            } else {
                Debug.Log("Attack");
                from.army.attack(to.army);
            }
        } else {  
            if (from.army == null ) {
                Debug.LogError("army is null");
            }          
            Debug.Log("3");
            from.army.Move(to);                        
        }
        State begin = new BeginTurnState();
        GameWorld.SetState(begin, -1);        
    }
    public override void OnStateExit() {
        GameWorld.PlayerMoove();
        Debug.Log("Exit Move State");
    }
        

   
}