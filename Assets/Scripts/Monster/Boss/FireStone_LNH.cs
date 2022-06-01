using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStone_LNH : MonoBehaviour
{
    
    public GameObject spawnRockPoint_L;
    public GameObject spawnRockPoint_R;
    public GameObject rock;
    public GameObject target;
    //public float speed = 500f;
    private GameObject fired_rock;
    private float initialAngel = 45f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(fired_rock != null)
        {
            fired_rock.transform.Rotate(new Vector3(1,1,1));
        }
        
    }

    void Shoot_L()
    {
        Vector3 velocity = GetVelocity(spawnRockPoint_L.transform.position, target.transform.position, initialAngel);
        fired_rock = Instantiate(rock, spawnRockPoint_L.transform.position, Quaternion.identity) as GameObject;
        fired_rock.GetComponent<Rigidbody>().velocity = velocity;


    }

    void Shoot_R()
    {
        Vector3 velocity = GetVelocity(spawnRockPoint_R.transform.position, target.transform.position, initialAngel);
        fired_rock = Instantiate(rock, spawnRockPoint_R.transform.position, Quaternion.identity) as GameObject;
        fired_rock.GetComponent<Rigidbody>().velocity = velocity;

        

    }

    public Vector3 GetVelocity(Vector3 spawnPos, Vector3 target, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 rockTarget = new Vector3(target.x, 0, target.z);
        Vector3 spawnPosition = new Vector3(spawnPos.x, 0, spawnPos.z);

        float distance = Vector3.Distance(rockTarget, spawnPosition);
        float yOffset = spawnPos.y - target.y;

        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, rockTarget - spawnPosition) * (target.x > spawnPos.x ? 1 : -1);
        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
}
