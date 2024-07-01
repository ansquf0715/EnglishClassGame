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
    GameObject ruleText;
    TMP_Text ruleTextComponent;

    Button nextButton;

    public List<GameObject> backObjects = new List<GameObject>();
    List<GameObject> instantiatedObjects = new List<GameObject>();
    List<GameObject> boxObjects = new List<GameObject>();
    List<GameObject> textObjects = new List<GameObject>();

    List<string> ruleSpeech = new List<string>();

    int rulePage = 0;

    bool[] opened = new bool[6];

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
        ruleText = Instantiate(textPrefab, parentTransform);
        ruleTextComponent = ruleText.GetComponent<TMP_Text>();
        Debug.Log("rule text" + ruleText);

        rulePage = 1;
        for (int i = 0; i < opened.Length; i++)
            opened[i] = false;
        opened[0] = true;
        opened[1] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(rulePage == 1)
        {
            rulePage1();
            //rulePage = -1;
        }
        else if(rulePage == 2)
        {
            rulePage2();
        }
        else if(rulePage == 3)
        {
            rulePage3();
        }
    }

    IEnumerator MoveObject(GameObject obj, Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = obj.transform.position;
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            obj.transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime/duration);
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

    IEnumerator TypeText(TMP_Text textObj, string textToType)
    {
        textObj.text = "";
        yield return new WaitForSeconds(1f);

        string displayText = textToType;
        foreach(char c in displayText)
        {
            textObj.text += c;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void nextButtonClicked()
    {
        for(int i=0; i<opened.Length; i++)
        {
            if(!opened[i])
            {
                rulePage = i;
                Debug.Log("rule page" + rulePage);
                opened[i] = true;
                break;
            }
        }
    }

    void rulePage1()
    {
        Debug.Log("여기 되나");

        StartCoroutine(TypeText(ruleTextComponent, ruleSpeech[0]));
        spawnNumberGrid(3, 3);
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        foreach (Transform child in canvas.transform)
        {
            Button button = child.GetComponent<Button>();
            if (child.name == "Next")
                nextButton = button;
        }
        nextButton.gameObject.SetActive(true);
        rulePage = -1;

    }

    void spawnNumberGrid(int rows, int columns)
    {
        float columnSpacing = 4f;
        float rowSpacing = 2f;

        int counter = 1;
        Vector2 startPos = new Vector2(-6f, 0f);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        for (int i=0; i<rows; i++)
        {
            for(int j=0; j<columns; j++)
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
        //ruleTextComponent.text = "";
        Debug.Log("rule page 2");
        StartCoroutine(TypeText(ruleTextComponent, ruleSpeech[1]));
        StartCoroutine(RemoveRandomObjects());
        rulePage = -1;
    }

    IEnumerator RemoveRandomObjects()
    {
        for (int i=0; i<6; i++)
        {
            int randomIndex = Random.Range(0, boxObjects.Count);
            Debug.Log("random Index" + randomIndex);
            Destroy(boxObjects[randomIndex]);
            Destroy(textObjects[randomIndex]);
            yield return new WaitForSeconds(1f);

        }
    }

    void rulePage3()
    {
        clearScreen();
        rulePage = -1;
    }

    void clearScreen()
    {
        for(int i=2; i<instantiatedObjects.Count; i++)
        {
            Destroy(instantiatedObjects[i]);
        }
        instantiatedObjects.RemoveRange(2, instantiatedObjects.Count - 2);

        foreach(GameObject obj in boxObjects)
        {
            Destroy(obj);
        }
        boxObjects.Clear();

        foreach(GameObject obj in textObjects)
        {
            Destroy(obj);
        }
        textObjects.Clear();
    }
}
