using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemyLogic : MonoBehaviour
{
    public Enemies enemies;
    [SerializeField]
    TextMeshProUGUI CurrentSize;
    int countersize = 0;
    float timer = 0;
    bool attack=true;

    int AttackTimer = 5;

    public static enemyLogic instance;
    // Start is called before the first frame update
    void Awake()
    {
        ChangeText(0);
        enemies.setEnemyStage(gameObject.name);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);

        if(seconds % AttackTimer == 0)
        {
            if(attack)
            {
                attack = false;
                Debug.Log(seconds);
            }
        }
        else
        {
            attack = true;
        }


    }

    public void ChangeText(int size)
    {
        countersize += size;
        CurrentSize.text = "Current size: " + countersize;
    }
}
