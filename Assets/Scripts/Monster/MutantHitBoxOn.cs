using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantHitBoxOn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject attackCollision1, attackCollision2, attackCollision3;

    public void OnAttackCollision1()
    {
        attackCollision1.SetActive(true);
    }

    public void OnAttackCollision2()
    {
        attackCollision2.SetActive(true);
    }

    public void OnAttackCollision3()
    {
        attackCollision3.SetActive(true);
    }
}
