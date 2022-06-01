using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemyByBoss_LNH : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnMonsterPoint1;
    public GameObject spawnMonsterPoint2;
    public GameObject monster;
    public GameObject particle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Summon()
    {
        Instantiate(monster, spawnMonsterPoint1.transform.position, Quaternion.identity);
        Instantiate(particle, spawnMonsterPoint1.transform.position, Quaternion.identity);
        Instantiate(monster, spawnMonsterPoint2.transform.position, Quaternion.identity);
        Instantiate(particle, spawnMonsterPoint2.transform.position, Quaternion.identity);
    }

    
}
