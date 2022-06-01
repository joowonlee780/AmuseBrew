using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierManage : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject chpivot;
    public GameObject chandelierObj;
    
    private bool isChandelierSwingLeft, isChandelierSwingRight;
    private bool isChandelierSwingOn;
    private float chandelierLeftAngle = -45f;
    private float chandelierRightAngle = 45f;

    private Quaternion targetLeftRotation;
    private Quaternion targetRightRotation;
    private float speed = 0.8f;

    private BossController_LNH bc;
    // Start is called before the first frame update
    void Start()
    {
        
        isChandelierSwingLeft = true; // 왼쪽으로 스윙, z축의 -45도까지 스윙
        isChandelierSwingRight = false; // 오른쪽으로 스윙, z축의 45도까지 스윙
        isChandelierSwingOn = true; // 샹들리에가 스윙 중이라는걸 알리는 bool
        chandelierObj = transform.GetChild(0).gameObject;

        bc = FindObjectOfType<BossController_LNH>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (bc.e_hp <= 20 && isChandelierSwingOn)
        {

            if (!isChandelierSwingRight) // 오른쪽으로 스윙하기
            {
                SwingRight();

            }
            else // 왼쪽으로 스윙하기
            {
                SwingLeft();
            }

        }

    }

    private void SwingRight()
    {
        targetRightRotation = Quaternion.Euler(0, 0, chandelierRightAngle);
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, targetRightRotation, speed * Time.deltaTime);

        if (this.transform.eulerAngles.z <= 39f && this.transform.eulerAngles.z >= 37f) SwitchSwingSide(); // z가 오일러각도 기준 37~39 사이에 들어오면 방향 전환
    }

    private void SwingLeft()
    {
        targetLeftRotation = Quaternion.Euler(0, 0, chandelierLeftAngle);
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, targetLeftRotation, speed * Time.deltaTime);

        if (this.transform.eulerAngles.z <= 323f && this.transform.eulerAngles.z >= 321f) SwitchSwingSide(); // z가 오일러각도 기준 321~323 사이에 들어오면 방향 전환
    }

    private void SwitchSwingSide()
    {
        isChandelierSwingRight = !isChandelierSwingRight;
        isChandelierSwingLeft = !isChandelierSwingLeft;
    }

    public void FallDown()
    {
        isChandelierSwingOn = false;
        chandelierObj.GetComponent<Rigidbody>().useGravity = true;
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        
        if(bc.e_hp <= 20 && collision.gameObject.tag == "BULLET")
        {           
            FallDown();
            Destroy(this.gameObject, 3f);
        }
    }

    
}
