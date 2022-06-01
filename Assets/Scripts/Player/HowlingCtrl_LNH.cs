using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowlingCtrl_LNH : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHowling()
    {
        ShakeCamera_LNH.Instance.OnShake(1f,1f);
    }
}
