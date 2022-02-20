using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    private Object[] Prefabs;
    private int counter=0;
    private int maxPieces = 50;
    Vector2 prevObject;
    List<GameObject> TouchedObjects;
    float speed = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TouchedObjects = new List<GameObject>();
        Prefabs=Resources.LoadAll("Prefabs", typeof(GameObject));
        generatePieces();
    }

    void generatePieces()
    {
        prevObject = new Vector2(0, 0);
        while(counter<maxPieces)
        {
            int pieceType = Random.Range(0, Prefabs.Length);
            Vector2 initialPos = Random.insideUnitCircle*2;
            GameObject tmp = Instantiate(Prefabs[pieceType], initialPos, Quaternion.identity) as GameObject;
            tmp.name += counter;
            tmp.GetComponent<puzzlepiece>().instatiateSelf();
            prevObject = initialPos;
            counter += 1;
        }

    }


    private void Update()
    {
        GameObject puzzlePiece=null;
        //check if the left mouse has been pressed down this frame
        if (Input.GetMouseButtonDown(0))
        {
            puzzlePiece= rayCastingObjects();
            if(puzzlePiece != null)
            {
                TouchedObjects.Add(puzzlePiece);
                Debug.Log(puzzlePiece.name);
            }
 
        }
        if (Input.GetMouseButton(0) && TouchedObjects.Count!=0)
        {
            puzzlePiece=rayCastingObjects();
            if (puzzlePiece!=null && !TouchedObjects.Contains(puzzlePiece))
            {
                if (puzzlePiece.GetComponent<puzzlepiece>().pieceType==TouchedObjects[TouchedObjects.Count-1].GetComponent<puzzlepiece>().pieceType)
                {
                    if(Vector2.Distance(puzzlePiece.transform.position, TouchedObjects[TouchedObjects.Count - 1].transform.position)<1.0f)
                    {
                        Debug.Log(puzzlePiece.name);
                        TouchedObjects.Add(puzzlePiece);
                    }

                }
            }  
            if(TouchedObjects[TouchedObjects.Count - 1])
            {
                //TouchedObjects[TouchedObjects.Count - 1].transform.position = Vector3.Lerp(TouchedObjects[TouchedObjects.Count - 1].transform.position, getMousePos(), speed * Time.deltaTime);
  
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            TouchedObjects.Clear();
            Debug.Log(TouchedObjects.Count);
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
