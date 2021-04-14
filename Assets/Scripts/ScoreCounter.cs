using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    private TextMeshProUGUI scoreCounterText;

   
    private void Awake()
    {
        Hitbox.ScoreIncreased += RunCo;
        scoreCounterText = GetComponent<TextMeshProUGUI>();
    }

     void Start()
    {
        GlobalVars.score = 0;
    }

    void Update()
    {
        scoreCounterText.text = "Points: " + GlobalVars.score.ToString();
    }

    //Increases score by x amount and scales text up and down to give a pulsating effect
    private IEnumerator Pulse()
    {
        for (float i = 1f; i <= 1.3f; i += 0.05f)
        {
            scoreCounterText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        scoreCounterText.rectTransform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

        GlobalVars.score = GlobalVars.score + 100;

        for (float i = 1f; i >= 1f; i -= 0.05f)
        {
            scoreCounterText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreCounterText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void RunCo()
    {
        StartCoroutine(Pulse());
    }

    private void OnDestroy()
    {
        Hitbox.ScoreIncreased -= RunCo;
    }
}
