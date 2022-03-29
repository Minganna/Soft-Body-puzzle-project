using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enemyType { Crab,None}


[CreateAssetMenu(fileName = "EnemyType", menuName = "Enemy Type", order = 51)]
public class Enemies : ScriptableObject
{
    [SerializeField]
    List<Sprite> enemySprites;
    GameObject thisOBJ;
    enemyType en; 


    public void setEnemyStage(string stage)
    {
        try
        {
            en = (enemyType)System.Enum.Parse(typeof(enemyType), stage);
            Debug.Log(en);
        }
        catch
        {
            en = enemyType.None;
            Debug.Log(en);
        }

    }
}
