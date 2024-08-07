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

    Button showButton;
    Button nextRoundButton;

    public List<GameObject> boxObjects = new List<GameObject>();
    public List<GameObject> textObjects = new List<GameObject>();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetUpRound();
        words = GetWords();
        ChangeBackGround();
        ChangeRoundText();
        SpawnGrid();
        SetTimer(time > 0 ? time : 5);
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

                GameObject textObject = Instantiate(GetWordPrefab().gameObject, canvas.transform);
                textObject.GetComponent<TMP_Text>().text = words[counter - 1];
                RectTransform textRect = textObject.GetComponent<RectTransform>();
                textRect.anchoredPosition = localCanvasPos;
                
                counter++;

                boxObjects.Add(boxObject);
                textObjects.Add(textObject);
            }
        }
    }

    void SetTimer(int time)
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        timer = canvas.transform.Find("Timer").gameObject;
        timer.SetActive(true);

        clock = Instantiate(GetClock(), new Vector2(3.85f, 4.25f), Quaternion.identity);

        slider = timer.GetComponent<Slider>();
        slider.maxValue = time;
        slider.value = 0f;

        StartCoroutine(UpdateTimer(time));
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
        ActivateShowButton();
    }

    void ActivateShowButton()
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        foreach(Transform child in canvas.transform)
        {
            Button button = child.GetComponent<Button>();
            if (child.name == "Show")
                showButton = button;
        }
        showButton.gameObject.SetActive(true);
        showButton.onClick.AddListener(showButtonclicked);
    }

    void showButtonclicked()
    {
        Debug.Log("remove words is called");
        StartCoroutine(RemoveRandomObjects());
    }

    IEnumerator RemoveRandomObjects()
    {
        for (int i = 0; i < 6; i++)
        {
            if (boxObjects.Count == 0 || textObjects.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, boxObjects.Count);

            Destroy(boxObjects[randomIndex]);
            boxObjects.RemoveAt(randomIndex);

            Destroy(textObjects[randomIndex]);
            textObjects.RemoveAt(randomIndex);

            yield return new WaitForSeconds(0.5f);
        }

        ActivateNextButton();
    }

    void ActivateNextButton()
    {
        showButton.gameObject.SetActive(false);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        foreach (Transform child in canvas.transform)
        {
            Button button = child.GetComponent<Button>();
            if (child.name == "NextRound")
                nextRoundButton = button;
        }
        nextRoundButton.gameObject.SetActive(true);
        //showButton.onClick.AddListener(showButtonclicked);
        nextRoundButton.onClick.AddListener(NextRoundButtonClicked);
    }

    void NextRoundButtonClicked()
    {
        Debug.Log("Next round button clicked called");
        ClearObjects();
        RoundManager roundManager = FindObjectOfType<RoundManager>();
        roundManager.changeCurrentRound(GetRoundNumber() + 1);
    }

    void ClearObjects()
    {
        foreach (var obj in boxObjects)
        {
            Destroy(obj);
        }
        boxObjects.Clear();

        foreach (var obj in textObjects)
        {
            Destroy(obj);
        }
        textObjects.Clear();

        Destroy(clock);

        nextRoundButton.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
    }
}
