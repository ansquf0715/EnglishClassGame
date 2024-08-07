using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Round : MonoBehaviour
{
    protected TMP_Text roundText;
    protected List<string> words = new List<string>();

    public int time;
    protected GameObject timer;
    protected Slider slider;
    protected GameObject clock;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetUpRound();
    }

    protected abstract void SetUpRound();
    protected abstract int GetRoundNumber();
    protected abstract List<string> GetWords();
    protected abstract Sprite GetBackGround();
    protected abstract GameObject GetWordPrefab();
    protected abstract GameObject GetWordBackGroundPrefab();
    protected abstract GameObject GetClock();

    void LoadWords()
    {
        words = GetWords();
    }

    void ChangeBackGround()
    {
        GameObject bgObject = GameObject.Find("BackGround");
        bgObject.transform.localScale = new Vector3(1.42f, 1.42f, 1f);
        SpriteRenderer sr = bgObject.GetComponent<SpriteRenderer>();
        sr.sprite = GetBackGround();
    }

    void ChangeRoundText()
    {
        roundText = GameObject.Find("RuleText").GetComponent<TMP_Text>();
        roundText.text = "ROUND" + GetRoundNumber();
        RectTransform rectTransform = roundText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-684, 358);
    }

    void SpawnGrid()
    {
        int rows = 3;
        int cols = 3;

        float columnSpacing = 6f;
        float rowSpacing = 2.8f;

        int counter = 1;
        Vector2 startPos = new Vector2(-6f, 2f);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        for(int i=0; i<rows; i++)
        {
            for(int j=0; j<cols; j++)
            {
                Vector2 position = startPos + new Vector2(j * columnSpacing, -i * rowSpacing);
                GameObject boxObject = Instantiate(GetWordBackGroundPrefab(), position, Quaternion.identity);

                Vector2 screenPosition = Camera.main.WorldToScreenPoint(boxObject.transform.position);
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();

                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,
                    screenPosition, null, out Vector2 localCanvasPos);

                GameObject textObject = Instantiate(GetWordPrefab(), canvas.transform);
                textObject.GetComponent<TMP_Text>().text = words[counter - 1];
                RectTransform textRect = textObject.GetComponent<RectTransform>();
                textRect.anchoredPosition = localCanvasPos;

                counter++;
            }
        }
    }

    void SetTimer(int duration)
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        timer = canvas.transform.Find("Timer").gameObject;
        timer.SetActive(false);

        clock = Instantiate(GetClock(), new Vector2(4.25f, 4.25f), Quaternion.identity);

        slider = timer.GetComponent<Slider>();
        slider.maxValue = duration;
        slider.value = 0f;

        
    }

    IEnumerator UpdateTimer(int duration)
    {
        for(int i=0; i<duration; i++)
        {
            slider.value += 1;
            yield return new WaitForSeconds(1f);
        }

        slider.value = slider.maxValue;
        OnTimerEnd();
    }

    void OnTimerEnd()
    {
        StartCoroutine(ShakeClock(1f));
    }

    IEnumerator ShakeClock(float duration)
    {
        Quaternion originalRot = clock.transform.rotation;
        float elapsedTime = 0f;
        float shakeAmount = 10f;

        while (elapsedTime < duration)
        {
            float angle = Random.Range(-shakeAmount, shakeAmount);
            clock.transform.rotation = originalRot * Quaternion.Euler(0, 0, angle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        clock.transform.rotation = originalRot;
    }

    void ActivateShowButton()
    {
        GameObject showButton = GameObject.Find("ShowButton");
        showButton.SetActive(true);
    }
}
