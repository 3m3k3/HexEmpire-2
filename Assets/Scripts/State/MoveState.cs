using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class MoveState : State {

    public int previousTarget; 
    public override void OnStateEnter(int idCell) {
        
        IHexCell from =  GameWorld.getCell(previousTarget);     
        IHexCell to =  GameWorld.getCell(idCell); 
        from.army.Move(to.transform.position);
    }
    public override void OnStateExit() {
        Debug.Log("Exit Move State");
    }
        

   
}