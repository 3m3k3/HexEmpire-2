using UnityEngine;


public class HexCity : IHexCell {


   
    public override bool isLand() {
        return true;
    }

    public override bool isSea() {
        return false;
    }

    public override bool isCity() {
        return true;
    }

    public override bool isPort() {
        return false;
    }

    public override bool isCapital() {
        return false;
    }
    
}