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
    LineRenderer lr;

    public type pieceType;
    // Start is called before the first frame update
    public void instatiateSelf()
    {
        lr = this.GetComponent<LineRenderer>();
        deactivateLineRender();
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


    public void connectToPrevious(Transform otherPos)
    {
        Color color = Color.white;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.enabled = true;
        lr.startColor=color;
        lr.endColor = color;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, otherPos.position);
    }

    public void deactivateLineRender()
    {
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, this.transform.position);
        lr.enabled = false;
    }

}
