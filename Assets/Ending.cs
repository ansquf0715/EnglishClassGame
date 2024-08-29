using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public Sprite backSprite;
    public List<GameObject> backObjects = new List<GameObject>();

    List<GameObject> instantiatedObjects = new List<GameObject>();

    protected TMP_Text roundText;

    Button homeButton;
    Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        changeBackSprite();
        showObjects();
        ShowText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void changeBackSprite()
    {
        GameObject bgObject = GameObject.Find("BackGround");
        bgObject.transform.localScale = new Vector3(1.9f, 1.9f, 1f);
        SpriteRenderer sr = bgObject.GetComponent<SpriteRenderer>();
        sr.sprite = backSprite;
    }

    void showObjects()
    {
        //대화창
        instantiatedObjects.Add(Instantiate(backObjects[0],
            new Vector2(0f, 2.4f), Quaternion.identity));

        //1번 사람
        GameObject temp = Instantiate(backObjects[1],
            new Vector2(-7.8f, -0.6f), Quaternion.identity);
        instantiatedObjects.Add(temp);
        temp.transform.localScale = new Vector3(1.4f, 1.4f, 0);

        //2번사람
        instantiatedObjects.Add(Instantiate(backObjects[2],
            new Vector2(-5.7f, -1.2f), Quaternion.identity));

        //체리
        temp = Instantiate(backObjects[3],
            new Vector2(-5f, -2.3f), Quaternion.identity);
        instantiatedObjects.Add(temp);
        temp.transform.localScale = new Vector3(0.2f, 0.2f, 1);

        //3번사람
        instantiatedObjects.Add(Instantiate(backObjects[4],
            new Vector2(-2.8f, -2.3f), Quaternion.identity));

        //야자수
        instantiatedObjects.Add(Instantiate(backObjects[5],
            new Vector2(-3.8f, -1.7f), Quaternion.identity));

        //가운데 사람
        instantiatedObjects.Add(Instantiate(backObjects[6],
            new Vector2(-0.3f, -3.06f), Quaternion.identity));

        //복숭아
        temp = Instantiate(backObjects[7],
            new Vector2(0.6f, -3.4f), Quaternion.identity);
        temp.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        instantiatedObjects.Add(temp);

        //가운데 위
        temp = Instantiate(backObjects[8],
            new Vector2(0, -0.65f), Quaternion.identity);
        temp.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        instantiatedObjects.Add(temp);

        //오른쪽 1번사람
        instantiatedObjects.Add(Instantiate(backObjects[9],
            new Vector2(2.6f, -2.3f), Quaternion.identity));

        //오른쪽 2번사람
        instantiatedObjects.Add(Instantiate(backObjects[10],
            new Vector2(5.3f, -1.5f), Quaternion.identity));

        //오렌지
        temp = Instantiate(backObjects[11],
            new Vector2(4f, -2.5f), Quaternion.identity);
        temp.transform.localScale = new Vector3(1f, 1f, 1f);
        instantiatedObjects.Add(temp);

        //오른쪽 3번사람
        temp = Instantiate(backObjects[12],
            new Vector2(7.6f, -0.8f), Quaternion.identity);
        temp.transform.localScale = new Vector3(1.4f, 1.4f, 1);
        instantiatedObjects.Add(temp);

        //사과
        temp = Instantiate(backObjects[13],
            new Vector2(8.1f, -1.7f), Quaternion.identity);
        temp.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        instantiatedObjects.Add(temp);
    }

    void ShowText()
    {
        roundText = GameObject.Find("RuleText").GetComponent<TMP_Text>();
        RectTransform rectTransform = roundText.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(0, 160);
        string textToType = "GOOD JOB!";

        StartCoroutine(TypeText(roundText, textToType));
    }

    IEnumerator TypeText(TMP_Text textObj, string textToType)
    {
        yield return new WaitForSeconds(0.5f);

        string displayText = textToType;
        foreach (char c in displayText)
        {
            textObj.text += c;
            yield return new WaitForSeconds(0.2f);
        }

        ShowButton();
    }

    void ShowButton()
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        foreach (Transform child in canvas.transform)
        {
            Button button = child.GetComponent<Button>();
            if (child.name == "Home")
                homeButton = button;
            else if(child.name == "Exit")
                exitButton = button;
        }

        homeButton.gameObject.SetActive(true);
        homeButton.onClick.AddListener(homeButtonClicked);

        exitButton.gameObject.SetActive(true);
        exitButton.onClick.AddListener(exitButtonClicked);
    }

    void homeButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void exitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void ClearScreen()
    {
        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();

        roundText.text = "";
        roundText.gameObject.SetActive(false);

        homeButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

}