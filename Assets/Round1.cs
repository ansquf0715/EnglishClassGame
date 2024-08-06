using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Round1 : MonoBehaviour
{
    public Sprite backGround;
    TMP_Text roundText;
    Image roundBack;

    List<string> words = new List<string>();

    public GameObject wordPrefab;
    public List<GameObject> backObjects = new List<GameObject>();
    
    List<GameObject> instantiatedObjects = new List<GameObject>();
    List<GameObject> boxObjects = new List<GameObject>();
    List<GameObject> textObjects = new List<GameObject>();

    public int time;
    GameObject timer;
    Slider slider;
    GameObject clock;

    // Start is called before the first frame update
    void Start()
    {
        GameObject bgObject = GameObject.Find("BackGround");
        bgObject.transform.localScale = new Vector3(1.42f, 1.42f, 1f);
        SpriteRenderer sr = bgObject.GetComponent<SpriteRenderer>();
        sr.sprite = backGround;

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        roundText = GameObject.Find("RuleText").GetComponent<TMP_Text>();
        roundText.text = "ROUND 1";
        RectTransform rectTransform = roundText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-684, 358);

        GameManager gameManager = FindObjectOfType<GameManager>();
        words = gameManager.getRoundWords(1);
        //Debug.Log("words" + words.Count);

        spawnNumberGrid(3, 3);

        if (time <= 0)
            time = 5;
        setTimer(time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnNumberGrid(int rows, int columns)
    {
          
        Debug.Log("spawn number grid is called");
        float columnSpacing = 6f;
        float rowSpacing = 2.8f;

        int counter = 1;
        Vector2 startPos = new Vector2(-6f, 02f);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector2 position = startPos + new Vector2(j * columnSpacing, -i * rowSpacing);
                GameObject boxObject = Instantiate(backObjects[0], position, Quaternion.identity);

                Vector2 screenPosition = Camera.main.WorldToScreenPoint(boxObject.transform.position);
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();

                Vector2 localCanvasPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,
                    screenPosition, null, out localCanvasPos);

                GameObject textObject = Instantiate(wordPrefab.gameObject, canvas.transform);
                textObject.GetComponent<TMP_Text>().text = words[counter-1];
                RectTransform textRect = textObject.GetComponent<RectTransform>();
                textRect.anchoredPosition = localCanvasPos;

                counter++;

                boxObjects.Add(boxObject);
                textObjects.Add(textObject);
            }
        }
    }

    void setTimer(int time)
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        timer = canvas.transform.Find("Timer").gameObject;
        timer.SetActive(true);

        clock = Instantiate(backObjects[1],
            new Vector2(4.25f, 4.25f), Quaternion.identity);
        instantiatedObjects.Add(clock);

        slider = timer.GetComponent<Slider>();
        slider.maxValue = time;
        slider.value = 0f;

        StartCoroutine(UpdateTime(time));
    }

    IEnumerator UpdateTime(int duration)
    {
        Debug.Log("update time");
        float timePerSecond = duration / (float)duration;
        for(int i=0; i<duration; i++)
        {
            slider.value += 1;
            yield return new WaitForSeconds(1f);
        }

        slider.value = slider.maxValue;
        OntimerEnd();
    }

    void OntimerEnd()
    {
        Debug.Log("timer is over");
        StartCoroutine(ShakeClock(1f));
    }

    IEnumerator ShakeClock(float duration)
    {
        Quaternion originalRot = clock.transform.rotation;
        float elapsedTime = 0f;
        float shakeAmount = 10f;

        while(elapsedTime < duration)
        {
            float angle = Random.Range(-shakeAmount, shakeAmount);
            clock.transform.rotation =
                originalRot * Quaternion.Euler(0, 0, angle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        clock.transform.rotation = originalRot;
    }
}
