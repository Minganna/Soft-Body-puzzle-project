using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzlepiece : MonoBehaviour
{
    public sprites selfsprt;
    float GameRadious = 1;
    // Start is called before the first frame update
    public void instatiateSelf()
    {
        selfsprt.setThisOBJ(this.gameObject);
        selfsprt.setSprite(); 
    }

}
