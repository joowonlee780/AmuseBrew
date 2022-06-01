using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class F1_MovetoMain : MonoBehaviour
{

    // can be set in the Inspector for this script
    public string sceneName;
    // alternatively, you can do something like
    // public const string sceneName = "SecondScene";

    // time after this script initializes, in seconds,
    // that the scene transition will happen
    public float TIME_LIMIT;

    // timer variable
    private float timer = 0F;
    // alternatively, you can set this in an Awake() function,
    // which is automatically called when the script initializes
    public Text subtitle;

    // automatically called many times every second
    void Update()
    {
        // deltaTime is the time (measured in seconds) since the previous Update step
        // it's typically very small, e.g. 1/60th of a second ~= 0.0167F
        this.timer += Time.deltaTime;

        EditingSubT(timer);
        // check if it's time to switch scenes
        if (this.timer >= TIME_LIMIT)
        {
            SceneManager.LoadScene(sceneName);
        }
    }


    private void EditingSubT(float timer)
    {
        if(timer >= 24f && timer <= 25.9f)
        {
            subtitle.text = "이번에도 수고많았네";
        }
        else if(timer >= 26f && timer <= 28f)
        {
            subtitle.text = "자세한 얘기는 앉아서 하게";
        }
        else if (timer >= 39f && timer <= 41f)
        {
            subtitle.text = "이곳에서 자네의 활약을 모두 지켜봤네";
        }
        else if (timer >= 41.3f && timer <= 44.5f)
        {
            subtitle.text = "자네의 생존에 대한 갈망은 항상 우리의 실험에 도움이 되는군";
        }
        else if (timer >= 44.6f && timer <= 47f)
        {
            subtitle.text = "다음 번에도 잘 부탁하네";
        }
        else if (timer >= 48f && timer <= 50f)
        {
            subtitle.text = "뭐? 집으로 보내달라고?";
        }
        else if (timer >= 56f && timer <= 58f)
        {
            subtitle.text = "뭔가 큰 착각을 하고 있나보군";
        }
        else if (timer >= 58.3f && timer <= 60f)
        {
            subtitle.text = "귀중한 실험체를 쉽게 보내줄 수 없지";
        }
        else if (timer >= 63.1f && timer <= 65f)
        {
            subtitle.text = "기억을 지우고 다음 실험 진행해";
        }
        else if (timer >= 78f && timer <= 80f)
        {
            subtitle.text = "다음 실험에서도 자네가 살아돌아오길 기대하지";
        }
        else
        {
            subtitle.text = "";
        }
    }

}
