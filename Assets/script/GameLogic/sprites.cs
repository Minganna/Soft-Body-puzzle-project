using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "puzzletype", menuName = "Puzzle Type", order = 51)]
public class sprites : ScriptableObject
{
    static int spriteSelected=0;
    [SerializeField]
    List<Sprite> puzzleSprites;
    GameObject thisOBJ;

    public void setThisOBJ(GameObject obj)
    {
        thisOBJ = obj;
    }

    // function used to set the sprite
    public void setSprite()
    {

        thisOBJ.GetComponent<SpriteRenderer>().sprite = puzzleSprites[spriteSelected];
    }



    public void setSpriteSelected(int selected)
    {
        spriteSelected = selected;
    }
}
