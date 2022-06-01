using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayTimeCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public Text playTime_text;
    private float time;
    private int playtime;
    private int minute;
    private float deadLine;

    private Playercontroller2_donghee pd;
    private PlayerHitManage pm;
    // Start is called before the first frame update
    void Start()
    {
        minute = 0;
        deadLine = 600;
        pd = FindObjectOfType<Playercontroller2_donghee>();
        pm = FindObjectOfType<PlayerHitManage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deadLine <= 0f)
        {
            pd.isDead = true;
            pm.hp = 0;
            return;
        }
        time += Time.deltaTime;
        deadLine -= Time.deltaTime;
        playtime = (int)deadLine % 60;
        minute = (int)deadLine / 60 % 60;

        if (playtime < 10)
        {
            playTime_text.text = $"{minute}:0{playtime}";
        }
        else
        {
            playTime_text.text = $"{minute}:{playtime}";
        }

    }
}
