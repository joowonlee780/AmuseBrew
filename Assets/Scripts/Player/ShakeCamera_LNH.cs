using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera_LNH : MonoBehaviour
{
    private static ShakeCamera_LNH instance;
    public static ShakeCamera_LNH Instance => instance;

    public float shakeTime;
    public float shakeIntensity;

    private Playercontroller2_donghee playerMove;
    private GunController gunController;
    public ShakeCamera_LNH()
    {
        // 자기 자신에 대한 정보를 static 형태의 변수에 저장해 외부에서 쉽게 접근 가능하게 함
        // 싱글턴 패턴 적용 부분
        instance = this;
    }

    private void Awake()
    {
        playerMove = GetComponent<Playercontroller2_donghee>();
        gunController = GetComponent<GunController>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 달리 설정하지 않으면 1초간 0.1의 세기로 카메라를 흔드는 함수
    // OnShake(0.5f) => 0.5초간 0.1의 세기로 카메라를 흔듬
    // OnShake(0.5f, 1f) => 0.5초간 1의 세기로 카메라를 흔듬
    public void OnShake(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeCameraOn");
        StartCoroutine("ShakeCameraOn");
    }

    IEnumerator ShakeCameraOn()
    {
        // 카메라 흔들림 연출 시작
        if(playerMove)playerMove.isShakeOn = true;
        if (gunController) gunController.isShakeOn = true;

        Vector3 startPos = transform.position;

        while (shakeTime > 0.0f)
        {
            float x_pos = Random.Range(-1f, 1f);

            transform.position = startPos + new Vector3(x_pos, 0, 0) * shakeIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPos;

        // 흔들림 연출 종료
        if(playerMove)playerMove.isShakeOn = false;
        if (gunController) gunController.isShakeOn = false;
    }
}
