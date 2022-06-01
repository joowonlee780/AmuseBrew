using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakRock_LNH : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject destroyEffectPrefab;    
    public AudioClip crashClip;
    
    public void PlayEffect()
    {
        Instantiate(destroyEffectPrefab, transform.localPosition, Quaternion.identity);       
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "PLAYER")
        {
            Debug.Log("Hit");
            PlayerHitManage pm = FindObjectOfType<PlayerHitManage>();
            pm.hp -= 5f;
            pm.Hit();
            
        }
        if (collision.gameObject.tag == "BULLET")
        {
            Debug.Log("Shot");
            Destroy(collision.gameObject);        
        }

        
        PlayEffect();
        AudioSource.PlayClipAtPoint(crashClip, collision.transform.position);
        
        Destroy(this.gameObject);
    }

}
