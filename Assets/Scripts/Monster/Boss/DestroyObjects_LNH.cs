using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects_LNH : MonoBehaviour
{
    // Start is called before the first frame update
    public float deleteTime = 3.0f;

    void Start()
    {
        //오브젝트를 생성한 후 deleteTime 만큼 시간이 경과하면 제거
        Destroy(gameObject, deleteTime);
    }

    
}
