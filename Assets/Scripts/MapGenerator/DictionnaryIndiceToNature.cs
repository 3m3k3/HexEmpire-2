using System.Collections.Generic;

  public enum Nature
    {
        Capital = - 1,
        City = -2,
        Port = -3,
        Sea_0 = 1,
        Sea_1 = 2,
        Sand_0 = 11,
        Sand_1 = 12
    } 
public static class DictionnaryIndiceToNature
{

    public static Nature getNature(int x) {
        if(x == -1) { return Nature.Capital;}
        if(x == -2) { return Nature.City;}
        if(x == -3) { return Nature.Port;}
        if(x < 450) { return Nature.Sea_1;}
        if(x < 1500) { return Nature.Sea_0;}
        if(x < 5000) { return Nature.Sand_0;}
        return Nature.Sand_1;
    }
    
    
}