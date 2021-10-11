using UnityEngine;


public class HexSand_0 : IHexCell {


   
    public override bool isLand() {
        return true;
    }

    public override bool isSea() {
        return false;
    }

    public override bool isCity() {
        return false;
    }

    public override bool isPort() {
        return false;
    }

    public override bool isCapital() {
        return false;
    }
    
}