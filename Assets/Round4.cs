using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Round4 : MonoBehaviour
{
    int currentRound = 4;
    List<string> round4sentence = new List<string>();

    TMP_Text roundText;
    public Sprite round4back;
    protected GameObject timer;
    public GameObject clock;
    protected Slider slider;

    Button showButton;
    Button nextRoundButton;
    public int time;

    public GameObject wordBack;
    public GameObject sentencePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        round4sentence = gameManager.getRoundSentence(currentRound);

        ChangeBackGround();
        ChangeRoundText();

        CreateWordBackObjects();
        CreateWordBackObjects();

        SetTimer(time > 0 ? time : 3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeBackGround()
    {
        GameObject bgObject = GameObject.Find("BackGround");
        bgObject.transform.localScale = new Vector3(1.42f, 1.42f, 1f);
        SpriteRenderer sr = bgObject.GetComponent<SpriteRenderer>();
        sr.sprite = round4back;
    }

    void ChangeRoundText()
    {
        roundText = GameObject.Find("RuleText").GetComponent<TMP_Text>();
        roundText.text = "ROUND" + currentRound;
        RectTransform rectTransform = roundText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-684, 358);
    }

    void SetTimer(int time)
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        timer = canvas.transform.Find("Timer").gameObject;
        timer.SetActive(true);

        clock = Instantiate(clock, new Vector2(3.85f, 4.25f), Quaternion.identity);

        slider = timer.GetComponent<Slider>();
        slider.maxValue = time;
        slider.value = 0f;

        StartCoroutine(UpdateTimer(time));
    }

    IEnumerator UpdateTimer(int duration)
    {
        for (int i = 0; i < duration; i++)
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
        foreach (Transform child in canvas.transform)
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
        //StartCoroutine(RemoveRandomObjects());
    }

    //IEnumerator RemoveRandomObjects()
    //{
    //    for (int i = 0; i < 6; i++)
    //    {
    //        if (boxObjects.Count == 0 || textObjects.Count == 0)
    //        {
    //            break;
    //        }

    //        int randomIndex = Random.Range(0, boxObjects.Count);

    //        Destroy(boxObjects[randomIndex]);
    //        boxObjects.RemoveAt(randomIndex);

    //        Destroy(textObjects[randomIndex]);
    //        textObjects.RemoveAt(randomIndex);

    //        yield return new WaitForSeconds(0.5f);
    //    }

    //    ActivateNextButton();
    //}

    void CreateWordBackObjects()
    {
        Vector2 startPos = new Vector2(0, 2);
        float yOffset = -2.7f;

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();


        for (int i=0; i<3; i++)
        {
            Vector2 pos = startPos + new Vector2(0, i*yOffset);
            GameObject boxObject = Instantiate(wordBack, pos, Quaternion.identity);

            Vector2 screenPosition = Camera.main.WorldToScreenPoint(boxObject.transform.position);
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,
                screenPosition, null, out Vector2 localCanvasPos);

            GameObject texTObject = Instantiate(sentencePrefab.gameObject, canvas.transform);
            texTObject.GetComponent<TMP_Text>().text = round4sentence[i];
            RectTransform textRect = texTObject.GetComponent<RectTransform>();
            textRect.anchoredPosition = localCanvasPos;
        }
    }

}
