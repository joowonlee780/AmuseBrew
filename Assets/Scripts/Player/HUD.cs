using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    [SerializeField] // 필요하면 HUD 호출, 필요 없으면 HUD 비활성화.
    private GameObject go_BulletHUD;

    [SerializeField] // 총알 갯수
    private Text[] text_Bullet;

    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.currentBulletCount.ToString();
        text_Bullet[1].text = currentGun.carryBulletCount.ToString();
    }
}
