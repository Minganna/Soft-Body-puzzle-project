using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum type { Pink, Blue, Green }
public class puzzlepiece : MonoBehaviour
{
    public sprites selfsprt;
    float GameRadious = 1;
    float scalesUp = 0;
    float scalingFramesLeft=0;
    Transform center;
    public bool alreadyTouched;

    public type pieceType;
    // Start is called before the first frame update
    public void instatiateSelf()
    {
        center = this.transform.GetChild(8);
        selfsprt.setThisOBJ(this.gameObject);
        selfsprt.setSprite();
    }

    private void Update()
    {
        if(scalingFramesLeft > 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x+scalesUp, transform.localScale.y + scalesUp, transform.localScale.z + scalesUp) , Time.deltaTime);
            scalingFramesLeft--;
        }

    }

    public void onSelected()
    {
        if(!alreadyTouched)
        {
            this.transform.position = center.position;
            foreach (Transform child in transform)
            {
                child.localPosition = child.GetComponent<BonePos>().startPos;
                child.rotation = child.GetComponent<BonePos>().startRot;
            }
        }
        alreadyTouched = true;
    }


    public void setScale(int counter)
    {
        scalesUp += counter;
        scalingFramesLeft = counter;
    }

    public Transform getCenter()
    {
        return center;
    }
    
    public int getSize()
    {
        return (int)scalesUp;
    }

}
