using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum type { Pink, Blue, Green }
public class puzzlepiece : MonoBehaviour
{
    public sprites selfsprt;
    float GameRadious = 1;

    public type pieceType;
    // Start is called before the first frame update
    public void instatiateSelf()
    {
        selfsprt.setThisOBJ(this.gameObject);
        selfsprt.setSprite(); 
    }

}
