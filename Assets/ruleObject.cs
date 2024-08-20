using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ruleObject : MonoBehaviour
{
    AudioSource bgm;
    public AudioClip bgmAudio;

    public GameObject textPrefab;
    public GameObject numberPrefab;
    public Transform parentTransform;
    //GameObject ruleText;
    TMP_Text ruleText;

    Button nextButton;
    Button previousButton;
    Button playButton;

    public List<GameObject> backObjects = new List<GameObject>();
    public List<GameObject> fruits = new List<GameObject>();

    public List<GameObject> instantiatedObjects = new List<GameObject>();
    List<GameObject> boxObjects = new List<GameObject>();
    List<GameObject> textObjects = new List<GameObject>();

    List<string> ruleSpeech = new List<string>();

    int rulePage = 1;

    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        bgm.clip = bgmAudio;
        bgm.Play();

        instantiatedObjects.Add(Instantiate(backObjects[0],
            new Vector2(5.7f, -1), Quaternion.identity));

        //대화창
        instantiatedObjects.Add(Instantiate(backObjects[1],
            new Vector2(-1.8f, 7f), Quaternion.identity));
        StartCoroutine(MoveObject(instantiatedObjects[1],
            new Vector2(-1.8f, 2.6f), 0.7f));

        setRuleText();

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        ruleText = GameObject.Find("RuleText").GetComponent<TMP_Text>();
        ruleText.text = "";

        rulePage = 1;

        foreach (Transform child in canvas.transform)
        {
            Button button = child.GetComponent<Button>();
            if (child.name == "Next")
                nextButton = button;
            else if (child.name == "Previous")
                previousButton = button;
            else if (child.name == "PlayButton")
                playButton = button;
        }
        nextButton.onClick.AddListener(nextButtonClicked);
        previousButton.onClick.AddListener(previousButtonClicked);
        ProcessRulePage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ProcessRulePage()
    {
        switch (rulePage)
        {
            case 1:
                rulePage1();
                break;
            case 2:
                rulePage2();
                break;
            case 3:
                rulePage3();
                break;
            case 4:
                rulePage4();
                break;
            case 5:
                rulePage5();
                break;
            case 6:
                rulePage6();
                break;
            default:
                Debug.Log("에러" + rulePage);
                break;
        }
    }

    IEnumerator MoveObject(GameObject obj, Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = obj.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition;
    }

    void setRuleText()
    {
        ruleSpeech.Add("Write 3 words.");
        ruleSpeech.Add("6 words will disappear.");
        ruleSpeech.Add("1 word = 1 fruit");
        ruleSpeech.Add("2 words = 3 fruits");
        ruleSpeech.Add("3 words = 5 fruits");
        ruleSpeech.Add("Let's go!");
    }

    IEnumerator TypeText(string textToType)
    {
        ruleText.text = "";
        //yield return new WaitForSeconds(1f);
        if (rulePage == 1)
            yield return new WaitForSeconds(1f);

        string displayText = textToType;
        foreach (char c in displayText)
        {
            ruleText.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void nextButtonClicked()
    {
        StopAllCoroutines();
        if(rulePage < 6)
        {
            rulePage++;
            ProcessRulePage();
        }
        //Debug.Log("rule page" + rulePage);
    }

    public void previousButtonClicked()
    {
        StopAllCoroutines();
        if(rulePage > 1)
        {
            rulePage--;
            ProcessRulePage();
        }
        //Debug.Log("rule page in previous" + rulePage);
    }

    void rulePage1()
    {
        clearBoxObjects();
        clearTextObjects();
        StartCoroutine(TypeText(ruleSpeech[0]));
        spawnNumberGrid(3, 3);
        nextButton.gameObject.SetActive(true);
        previousButton.gameObject.SetActive(true);
    }

    void spawnNumberGrid(int rows, int columns)
    {
        float columnSpacing = 4f;
        float rowSpacing = 2f;

        int counter = 1;
        Vector2 startPos = new Vector2(-5f, 0f);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector2 position = startPos + new Vector2(j * columnSpacing, -i * rowSpacing);
                GameObject boxObject = Instantiate(backObjects[2], position, Quaternion.identity);

                Vector2 screenPosition = Camera.main.WorldToScreenPoint(boxObject.transform.position);
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();

                Vector2 localCanvasPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,
                    screenPosition, null, out localCanvasPos);

                GameObject textObject = Instantiate(numberPrefab.gameObject, canvas.transform);
                textObject.GetComponent<TMP_Text>().text = counter.ToString();
                RectTransform textRect = textObject.GetComponent<RectTransform>();
                textRect.anchoredPosition = localCanvasPos;

                counter++;

                boxObjects.Add(boxObject);
                textObjects.Add(textObject);
            }
        }
    }

    void rulePage2()
    {
        clearBoxObjects();
        clearTextObjects();
        spawnNumberGrid(3, 3);

        StartCoroutine(TypeText(ruleSpeech[1]));
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
    }

    void rulePage3()
    {
        for(int i=0; i<boxObjects.Count; i++)
        {
            Destroy(boxObjects[i]);
            Destroy(textObjects[i]);
        }
        boxObjects.Clear();
        textObjects.Clear();

        StartCoroutine(TypeText(ruleSpeech[2]));
        GameObject boxObject = Instantiate(backObjects[2],
            new Vector2(-1.5f, -1.7f), Quaternion.identity);
        boxObject.transform.localScale = new Vector3(0.6f, 0.7f, 1f);
        boxObjects.Add(boxObject);

        //Calculate the center of the boxObject
        Vector3 boxCenter = boxObject.transform.position;
        Vector3 boxSize = boxObject.transform.localScale;
        Vector3 centerPosition = boxCenter;

        GameObject pear = Instantiate(fruits[0], centerPosition, Quaternion.identity);
        pear.transform.parent = boxObject.transform;
    }

    void clearBoxObjects()
    {
        for(int i=0; i<boxObjects.Count; i++)
        {
            Destroy(boxObjects[i]);
        }
        boxObjects.Clear();
    }

    void clearTextObjects()
    {
        for(int i=0; i<textObjects.Count; i++)
        {
            Destroy(textObjects[i]);
        }
        textObjects.Clear();
    }

    void rulePage4()
    {
        clearBoxObjects();
        StartCoroutine(TypeText(ruleSpeech[3]));

        Vector2 startPosition = new Vector2(2.5f, -1.2f);
        float spacing = 3.8f;

        for(int i=0; i<3; i++)
        {
            Vector2 pos = startPosition + new Vector2(-spacing * i, 0);
            GameObject boxObject = Instantiate(backObjects[2],
                pos, Quaternion.identity);
            boxObject.transform.localScale = new Vector3(0.5f, 0.6f, 1f);
            boxObjects.Add(boxObject);

            GameObject fruit = Instantiate(fruits[i], pos, Quaternion.identity);
            fruit.transform.parent = boxObject.transform;
        }
    }

    void rulePage5()
    {
        clearBoxObjects();
        StartCoroutine(TypeText(ruleSpeech[4]));

        Vector2 bottom = new Vector2(2.4f, -3.5f);
        float horizontal = 4.5f;
        float vertical = 2.8f;

        Vector2[] bottomPos = new Vector2[3];
        for(int i=0; i<3; i++)
        {
            Vector2 pos = bottom + new Vector2(-horizontal * i, 0);
            bottomPos[i] = pos;
            GameObject boxObject = Instantiate(backObjects[2], pos, Quaternion.identity);
            boxObject.transform.localScale = new Vector3(0.6f, 0.7f, 1f);
            boxObjects.Add(boxObject);

            GameObject fruit = Instantiate(fruits[i], pos, Quaternion.identity);
            fruit.transform.parent = boxObject.transform;
        }

        Vector2 pos4 = (bottomPos[0] + bottomPos[1]) / 2 + new Vector2(0, vertical);
        Vector2 pos5 = (bottomPos[1] + bottomPos[2]) / 2 + new Vector2(0, vertical);
        Vector2[] topPos = new Vector2[] { pos4, pos5 };
        for (int i = 0; i < 2; i++)
        {
            GameObject boxObject = Instantiate(backObjects[2],
                topPos[i], Quaternion.identity);
            boxObject.transform.localScale = new Vector3(0.6f, 0.7f, 1f);
            boxObjects.Add(boxObject);

            int fruitIndex = i + 3;
            GameObject fruit = Instantiate(fruits[fruitIndex],
                topPos[i], Quaternion.identity);
            fruit.transform.parent = boxObject.transform;
        }
    }

    void rulePage6()
    {
        clearBoxObjects();
        clearScreen();
        nextButton.gameObject.SetActive(false);
        previousButton.gameObject.SetActive(false);

        RectTransform ruleTextRect = ruleText.GetComponent<RectTransform>();
        ruleTextRect.anchoredPosition = new Vector2(0, 130);
        StartCoroutine(TypeText(ruleSpeech[5]));

        //대화창
        instantiatedObjects.Add(Instantiate(backObjects[1],
            new Vector2(0f, 2f), Quaternion.identity));

        float startX = -6f;
        float horizontal = 3f;

        float[] yPos = new float[] { -1f, -2f, -2.5f, -2f, -1f };

        for(int i=0; i<5; i++)
        {
            Vector2 pos = new Vector2(startX + horizontal * i, yPos[i]);
            GameObject obj = Instantiate(backObjects[i + 3], pos, Quaternion.identity);
            instantiatedObjects.Add(obj);
        }

        playButton.gameObject.SetActive(true);
        playButton.transform.SetAsLastSibling();
    }

    void clearScreen()
    {
        //ruleText.text = " ";

        foreach(GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();

        foreach (GameObject obj in boxObjects)
        {
            Destroy(obj);
        }
        boxObjects.Clear();

        foreach (GameObject obj in textObjects)
        {
            Destroy(obj);
        }
        textObjects.Clear();

        playButton.gameObject.SetActive (false);
    }

    private void OnDestroy()
    {
        clearScreen();
    }
}
