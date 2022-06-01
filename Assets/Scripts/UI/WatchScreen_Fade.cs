using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchScreen_Fade : MonoBehaviour
{
    private CanvasGroup watch_cg;
    public float fadeTime = 1f;
    float delayTime = 0f;

    private Coroutine fadeCor;

    // Start is called before the first frame update
    private void Awake()
    {
        watch_cg = this.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartFadeIn();
    }

    private void OnDisable()
    {
        watch_cg.alpha = 0f;
    }

    public void StartFadeIn()
    {
        if (fadeCor != null) // 이전에 실행되고 있던 코루틴이 있을 시
        {
            StopAllCoroutines(); // 모든 코루틴 종료
            fadeCor = null; // null로 바꿔줌
        }
        fadeCor = StartCoroutine(FadeIn());

    }



    private IEnumerator FadeIn()
    {
        delayTime = 0f;
        while (delayTime < fadeTime)
        {
            watch_cg.alpha = Mathf.Lerp(0f, 1f, delayTime / fadeTime); // 선형 보간으로 부드럽게 페이드인 되는 연출
            yield return 0;
            delayTime += Time.deltaTime;
        }
        watch_cg.alpha = 1f;
    }
}
