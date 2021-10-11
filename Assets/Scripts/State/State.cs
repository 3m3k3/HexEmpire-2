using UnityEngine;

public abstract class State {
    public GameWorld GameWorld;

  
    public abstract void OnStateEnter(int idCell);
    public abstract void OnStateExit(); 

    public void init(GameWorld GameWorld) {
        this.GameWorld = GameWorld;
    }

}