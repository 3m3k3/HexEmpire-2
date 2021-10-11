using UnityEngine;


public class HexSea_1 : IHexCell {



   

    public override bool isLand() {
        return false;
    }

    public override bool isSea() {
        return true;
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