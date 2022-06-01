using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private bool isReload = false;

    private AudioSource audioSource;
    private AudioSource Reload_audioSource;

    // 필요한 컴포넌트
    [SerializeField]
    private CrossHair theCrossshair;
    [SerializeField]
    private GameObject PistolBullet;
    [SerializeField]
    private Transform FireSpot;
    [SerializeField]
    private float Bullet_speed;

    [SerializeField]
    private Vector3 originPos;
    [SerializeField]
    private Vector3 originRotation;

    public bool isShakeOn; // 카메라 흔들림 연출 확인용 변수

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Reload_audioSource = GetComponent<AudioSource>();
        theCrossshair = FindObjectOfType<CrossHair>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameisPause) return;
        if (isShakeOn) return; // 카메라가 흔들리고 있으면 잠시 움직임 제어 멈춤
        GunFireRateCalc();
        TryFire();
        TryReload();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; // 60분의 1
    }

    private void TryFire()
    {
        if (Input.GetMouseButtonDown(0) && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
                StartCoroutine(ReloadCoroutine());
        }
        

    }

    private void Shoot()
    {
        theCrossshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // 발사 속도 재계산
        PlaySE(currentGun.fire_Sound);
        //currentGun.muzzleFlash.Play();
        var a = Instantiate(PistolBullet, FireSpot.position, FireSpot.rotation);
        a.GetComponent<Rigidbody>().AddForce(FireSpot.transform.forward*Bullet_speed);

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

        


        Debug.Log("총알 발사됨");
        //Debug.Log(PistolBullet.gameObject.tag);
        Destroy(a.gameObject, 2.0f);
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            PlayRE(currentGun.reload_Sound);
            StartCoroutine(ReloadCoroutine());
        }
    }
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.Play("Reloading");
            PlayRE(currentGun.reload_Sound);

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }


            isReload = false;

        }

        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }

    IEnumerator RetroActionCoroutine() //총기 반동
    {
        float rate = currentFireRate / 2;
        double z = 0;
        Quaternion current = currentGun.transform.rotation;
        
        while (z < 0.25)
        {
            currentGun.transform.Rotate(new Vector3(0, 0, 2));
            z = z + 0.0166;
            yield return null;
        }

        while (z < 0.5)
        {
            currentGun.transform.Rotate(new Vector3(0, 0, -2));
            z = z + 0.0166;
            yield return null;
        }
        currentGun.transform.localEulerAngles = new Vector3(current.x, current.y, 0);



    }


    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    private void PlayRE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

}
