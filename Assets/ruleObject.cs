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

    public List<GameObject> backObjects = new List<GameObject>();
    List<GameObject> instantiatedObjects = new List<GameObject>();
    List<GameObject> boxObjects = new List<GameObject>();
    List<GameObject> textObjects = new List<GameObject>();

    List<string> ruleSpeech = new List<string>();

    int rulePage;
    bool isProcessed = false;

    //bool[] opened = new bool[6];

    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        bgm.clip = bgmAudio;
        bgm.Play();

        instantiatedObjects.Add(Instantiate(backObjects[0],
            new Vector2(5.5f, -1), Quaternion.identity));

        //대화창
        instantiatedObjects.Add(Instantiate(backObjects[1],
            new Vector2(-1.8f, 7f), Quaternion.identity));
        StartCoroutine(MoveObject(instantiatedObjects[1],
            new Vector2(-1.8f, 2.6f), 1.5f));

        setRuleText();
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        ruleText = GameObject.Find("RuleText").GetComponent<TMP_Text>();
        Debug.Log("rulet Text = " + ruleText);
        ruleText.text = "";

        Debug.Log("rule text" + ruleText);

        rulePage = 1;

        foreach (Transform child in canvas.transform)
        {
            Button button = child.GetComponent<Button>();
            if (child.name == "Next")
                nextButton = button;
        }

        ProcessRulePage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ProcessRulePage()
    {
        Debug.Log("process rule page");
        isProcessed = false;
        switch (rulePage)
        {
            case 1:
                rulePage1();
                break;
            case 2:
                Debug.Log("case 2");
                rulePage2();
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
        yield return new WaitForSeconds(1f);

        string displayText = textToType;
        foreach (char c in displayText)
        {
            ruleText.text += c;
            yield return new WaitForSeconds(0.2f);
        }

        isProcessed = true;
    }

    public void nextButtonClicked()
    {
        //if(!isProcessed)
        //{
        //    return;
        //}
        StopAllCoroutines();

        Debug.Log("rule page 현재" + rulePage);

        rulePage++;
        Debug.Log("rule page" + rulePage);

        Debug.Log("rule page ++ 된 후" + rulePage);
        ProcessRulePage();
    }

    void rulePage1()
    {
        isProcessed = false;
        StartCoroutine(TypeText(ruleSpeech[0]));
        spawnNumberGrid(3, 3);
        nextButton.gameObject.SetActive(true);
    }

    void spawnNumberGrid(int rows, int columns)
    {
        float columnSpacing = 4f;
        float rowSpacing = 2f;

        int counter = 1;
        Vector2 startPos = new Vector2(-6f, 0f);

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
        Debug.Log("나는 2번 화면이다");
        //rule text가 null이여서 실행이 안된이슈 해결
        //ruleText.text = "";
        isProcessed = false;
        StartCoroutine(TypeText(ruleSpeech[1]));
        StartCoroutine(RemoveRandomObjects());
    }

    IEnumerator RemoveRandomObjects()
    {
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, boxObjects.Count);
            Debug.Log("random Index" + randomIndex);
            Destroy(boxObjects[randomIndex]);
            Destroy(textObjects[randomIndex]);
            yield return new WaitForSeconds(1f);

        }

        isProcessed = true;
    }

    void rulePage3()
    {
        isProcessed = true;
        //clearScreen();
    }

    void clearScreen()
    {
        for (int i = 2; i < instantiatedObjects.Count; i++)
        {
            Destroy(instantiatedObjects[i]);
        }
        instantiatedObjects.RemoveRange(2, instantiatedObjects.Count - 2);

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
    }
}
