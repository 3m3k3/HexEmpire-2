using UnityEngine;


public class HexPort : IHexCell {


   
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
        return true;
    }

    public override bool isCapital() {
        return false;
    }
    
}