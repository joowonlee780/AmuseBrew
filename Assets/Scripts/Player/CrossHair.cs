using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    private float gunAccuracy; // 정확도

    [SerializeField]
    private GameObject go_CrosshairHUD; // 크로스헤어 비활성화 용

   


    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("Walking"))
            animator.SetTrigger("Fire");
        else if (animator.GetBool("Running"))
            animator.SetTrigger("Fire");
        else
            animator.SetTrigger("Fire");
    }
}
