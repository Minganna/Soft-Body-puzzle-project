using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Object[] Prefabs;
    private int objcounter=0;
    private int maxPieces = 50;
    Vector2 prevObject;
    List<GameObject> allObjects;
    List<GameObject> TouchedObjects;
    List<string> nameToRemove;
    float speed = 0.1f;

    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TouchedObjects = new List<GameObject>();
        allObjects= new List<GameObject>();
        Prefabs =Resources.LoadAll("Prefabs", typeof(GameObject));
        generatePieces();
    }

    void generatePieces()
    {
        prevObject = new Vector2(0, 0);
        while(objcounter < maxPieces)
        {
            int pieceType = Random.Range(0, Prefabs.Length);
            Vector2 initialPos = Random.insideUnitCircle*2;
            GameObject tmp = Instantiate(Prefabs[pieceType], initialPos, Quaternion.identity) as GameObject;
            tmp.name += objcounter;
            tmp.GetComponent<puzzlepiece>().instatiateSelf();
            allObjects.Add(tmp);
            prevObject = initialPos;
            objcounter += 1;
        }

    }


    private void Update()
    {
        GameObject puzzlePiece=null;
        //check if the left mouse has been pressed down this frame
        if (Input.GetMouseButtonDown(0))
        {
            puzzlePiece = rayCastingObjects();
          
            if (puzzlePiece != null)
            {
                puzzlePiece.GetComponent<puzzlepiece>().onSelected();
                TouchedObjects.Add(puzzlePiece);
                Color color = Color.gray;
                changeSpriteColor(puzzlePiece, color);
            }

        }
        if (Input.GetMouseButton(0) && TouchedObjects.Count!=0)
        {
            puzzlePiece=rayCastingObjects();
            if (puzzlePiece!=null && !TouchedObjects.Contains(puzzlePiece))
            {
                puzzlepiece pp = puzzlePiece.GetComponent<puzzlepiece>();
                GameObject previousOBJ = TouchedObjects[TouchedObjects.Count - 1];
                puzzlepiece previousPp = previousOBJ.GetComponent<puzzlepiece>();
                if (pp.pieceType== previousPp.pieceType)
                {
                    pp.onSelected();
                    if (Vector2.Distance(puzzlePiece.transform.position, previousOBJ.transform.position)< (pp.getSize()+previousPp.getSize())+1.2f)
                    {
                        pp.connectToPrevious(previousOBJ.transform);
                        TouchedObjects.Add(puzzlePiece);
                    }

                }
            }  
        }
        if(Input.GetMouseButtonUp(0))
        {
            Color color = Color.white;
            if (TouchedObjects.Count>0)
            {
                changeSpriteColor(TouchedObjects[TouchedObjects.Count - 1], color);
            }

            int counter = 0;
            foreach(GameObject obj in TouchedObjects)
            {
                puzzlepiece pzp = obj.GetComponent<puzzlepiece>();
                pzp.alreadyTouched = false;
                counter++;
                changeSpriteColor(obj, color);
                if (TouchedObjects.Count>1)
                {
                    if (counter < TouchedObjects.Count)
                    {
                        allObjects.Remove(obj);
                        Destroy(obj);
                    }
                    else
                    {
                        pzp.setScale(counter);
                        pzp.deactivateLineRender();
                        pzp.onSelected();
                    }
                }
                else
                {
                    allObjects.Remove(obj);
                    Destroy(obj);
                }
            }
            TouchedObjects.Clear();
            objcounter = 0;
            foreach(GameObject objs in allObjects)
            {
                puzzlepiece tmp = objs.GetComponent<puzzlepiece>();
                if(tmp.getSize()==0)
                {
                    objcounter++;
                }
                else
                {
                    objcounter += tmp.getSize();
                }
            }
            generatePieces();
        }
    }

    private void changeSpriteColor(GameObject puzzlePiece, Color color)
    {
        foreach (GameObject piece in allObjects)
        {
            if (piece.GetComponent<puzzlepiece>().pieceType != puzzlePiece.GetComponent<puzzlepiece>().pieceType)
            {
                SpriteRenderer pzp = piece.GetComponent<SpriteRenderer>();
                pzp.color = color;
            }
        }
    }

    private static GameObject rayCastingObjects()
    {
        Vector3 ray = getMousePos();

        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

        if (hit.collider != null)
        {
            try
            {
                GameObject parent = hit.collider.transform.parent.gameObject;
                //print out the name of parent if the raycast hits something
                return parent;
            }
            catch
            {
                return null;
            }

        }
        return null;
    }

    private static Vector3 getMousePos()
    {
        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ray.z = 0.0f;
        return ray;
    }
}
