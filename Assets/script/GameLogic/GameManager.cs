using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Object[] Prefabs;
    private int counter=0;
    private int maxPieces = 50;
    Vector2 prevObject;
    float radious = 50.0f;
    
    // Start is called before the first frame update
    void Start()
    {
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
            tmp.GetComponent<puzzlepiece>().instatiateSelf();
            prevObject = initialPos;
            counter += 1;
        }

    }

}
