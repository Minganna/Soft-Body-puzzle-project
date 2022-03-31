using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Object[] Prefabs;
    private int objcounter=0;
    private int maxPieces = 50;
    Vector2 prevObject;
    List<GameObject> allObjects;
    List<GameObject> TouchedObjects;
    List<Image> UIsprites;
    List<string> nameToRemove;
    float speed = 0.1f;
    enemyLogic el;

    
    // Start is called before the first frame update
    void Start()
    {  
        instance = this;
        TouchedObjects = new List<GameObject>();
        allObjects= new List<GameObject>();
        Prefabs =Resources.LoadAll("Prefabs/PuzzlePieces", typeof(GameObject));
        UIsprites = new List<Image>();
        GameObject[] tmpUI= GameObject.FindGameObjectsWithTag("UISprites");
        foreach(GameObject ui in tmpUI)
        {
            UIsprites.Add(ui.GetComponent<Image>());
        }
        generatePieces();
        el = enemyLogic.instance;
        getIcons();
    }

    void getIcons()
    {
        int spritesCount = 0;
        List<Sprite> sprites = new List<Sprite>();
        foreach (GameObject pp in allObjects)
        {
            puzzlepiece tmpPuzzlepiece = pp.GetComponent<puzzlepiece>();
            if (!sprites.Contains(tmpPuzzlepiece.GetSprite()))
            {
                if (spritesCount < UIsprites.Count)
                {
                    sprites.Add(tmpPuzzlepiece.GetSprite());
                    UIsprites[spritesCount].sprite = sprites[spritesCount];
                    el.setPuzzles(tmpPuzzlepiece.GetSprite().name.ToLower());
                    spritesCount++;
                }
            }
        }
        el.addBonus();
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
                    if (Vector2.Distance(puzzlePiece.transform.position, previousOBJ.transform.position)<1.2f)
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
                    pzp.deactivateLineRender();
                    pzp.onSelected(); 
                    counter++;
                    allObjects.Remove(obj);
                    Destroy(obj);
                }
                else
                {
                    allObjects.Remove(obj);
                    Destroy(obj);
                }
            }
            el.ChangeText(counter);
            TouchedObjects.Clear();
            objcounter = allObjects.Count;

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


    public void takeDamage()
    {
        maxPieces--;
        int puzzleToRemove = Random.Range(0, allObjects.Count-1);
        if (!TouchedObjects.Contains(allObjects[puzzleToRemove]))
        {
            GameObject tmp = allObjects[puzzleToRemove];
            allObjects.Remove(tmp);
            Destroy(tmp);
        }
    }

    private static Vector3 getMousePos()
    {
        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ray.z = 0.0f;
        return ray;
    }
}
