using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Sentence : MonoBehaviour
{
    protected TMP_Text roundText;
    protected List<string> sentences = new List<string>();

    public int time;
    protected GameObject timer;
    protected Slider slider;
    protected GameObject clock;
    protected GameObject dice;
    protected GameObject star;


    Button showButton;
    Button nextRoundButton;

    public List<GameObject> boxObjects = new List<GameObject>();
    public List<GameObject> textObjects = new List<GameObject>();
    public List<GameObject> stars = new List<GameObject>();

    AudioSource effectsSource;
    //0: 단어 1:알람 2:주사위
    public AudioClip[] effectClips;

    // Start is called before the first frame update
    void Start()
    {
        star = Resources.Load<GameObject>("강조 별");

        effectsSource = GameObject.Find("Effect Audio Source").GetComponent<AudioSource>();
        
        SetUpRound();
        sentences = GetSentences();
        ChangeBackGround();
        ChangeRoundText();
        SpawnSentences();

        GameManager gm = FindObjectOfType<GameManager>();
        SetTimer(gm.time);
        //SetTimer(time > 0 ? time : 20);

        effectClips = Resources.LoadAll<AudioClip>("SoundEffects/");
    }

    protected abstract void SetUpRound();
    protected abstract int GetRoundNumber();
    protected abstract List<string> GetSentences();
    protected abstract Sprite GetBackGround();
    protected abstract GameObject GetSentencePrefab();
    protected abstract GameObject GetSentenceBackGroundPrefab();
    protected abstract GameObject GetClock();

    protected abstract GameObject GetDice();


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
        RectTransform rectTransform = roundText.GetComponent<RectTransform>();

        if (GetRoundNumber() == 8)
        {
            roundText.text = "FINAL!!!";
            rectTransform.anchoredPosition = new Vector2(-650, 358);
        }
        else
        {
            roundText.text = "ROUND" + GetRoundNumber();
            rectTransform.anchoredPosition = new Vector2(-684, 358);
        }
    }

    void SpawnSentences()
    {
        Vector2 startPos = new Vector2(0, 2);
        float yOffset = -2.7f;

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        for(int i=0; i<3; i++)
        {
            Vector2 pos = startPos + new Vector2(0, i * yOffset);
            GameObject box = Instantiate(GetSentenceBackGroundPrefab(),
                pos, Quaternion.identity);

            Vector2 screenPos = Camera.main.WorldToScreenPoint(box.transform.position);
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,
                screenPos, null, out Vector2 localCanvasPos);

            GameObject textObj = Instantiate(GetSentencePrefab().gameObject, canvas.transform);
            textObj.GetComponent<TMP_Text>().text = sentences[i];
            RectTransform textRect = textObj.GetComponent<RectTransform>();

            textRect.anchoredPosition = localCanvasPos;

            boxObjects.Add(box);
            textObjects.Add(textObj);
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
        effectsSource.clip = effectClips[1];
        effectsSource.Play();

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
        SpawnDice();
    }

    void SpawnDice()
    {
        dice = Instantiate(GetDice(), new Vector2(7.0f, 0f), Quaternion.identity);
        effectsSource.clip = effectClips[2];
        effectsSource.Play();
        StartCoroutine(delayButton());
    }

    IEnumerator delayButton()
    {
        yield return new WaitForSeconds(1.2f);
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

        showButton.interactable = true;
        showButton.onClick.AddListener(showButtonclicked);
    }

    void showButtonclicked()
    {
        StartCoroutine(RemoveRandomObjects());
        showButton.interactable=false;
    }

    IEnumerator RemoveRandomObjects()
    {
        for(int i=0; i<2; i++)
        {
            if (boxObjects.Count == 0 || textObjects.Count == 0)
                break;
            int randomIndex = Random.Range(0, boxObjects.Count);

            Destroy(boxObjects[randomIndex]);
            boxObjects.RemoveAt(randomIndex);

            Destroy(textObjects[randomIndex]);
            textObjects.RemoveAt(randomIndex);

            effectsSource.clip = effectClips[0];
            effectsSource.Play();

            yield return new WaitForSeconds(0.5f);
        }

        SpawnStars();
        ActivateNextButton();
    }

    void SpawnStars()
    {
        if (boxObjects.Count == 1)
        {
            foreach (GameObject box in boxObjects)
            {
                Transform boxTransform = box.transform;

                SpriteRenderer boxRenderer = box.GetComponent<SpriteRenderer>();
                Vector3 boxSize = boxRenderer.bounds.size;

                Vector3 topLeft
                    = boxTransform.position + new Vector3(-boxSize.x / 2, boxSize.y / 2, 0);
                Vector3 bottomRight
                    = boxTransform.position + new Vector3(boxSize.x / 2, -boxSize.y / 2, 0);

                float offset = 0.6f;

                GameObject starTopLeft
                    = Instantiate(star, topLeft + new Vector3(offset, -offset, 0), Quaternion.identity);
                GameObject starBottomRight
                    = Instantiate(star, bottomRight + new Vector3(-offset, offset, 0), Quaternion.identity);

                starTopLeft.transform.rotation = Quaternion.Euler(0, 0, -90);
                starBottomRight.transform.rotation = Quaternion.Euler(0, 0, 90);

                stars.Add(starTopLeft);
                stars.Add(starBottomRight);


                float starHeight = star.GetComponent<SpriteRenderer>().bounds.size.y;
                starTopLeft.transform.localPosition = new Vector3(starTopLeft.transform.localPosition.x,
                    starTopLeft.transform.localPosition.y + starHeight / 2,
                    starTopLeft.transform.localPosition.z);

                starBottomRight.transform.localPosition = new Vector3(starBottomRight.transform.localPosition.x,
                    starBottomRight.transform.localPosition.y - starHeight / 2,
                    starBottomRight.transform.localPosition.z);
            }

            StartCoroutine(BlinkStars(1f, 0.1f));
        }
    }

    IEnumerator BlinkStars(float duration, float interval)
    {
        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < duration)
        {
            foreach (GameObject star in stars)
            {
                star.SetActive(isVisible);
            }

            isVisible = !isVisible;
            elapsedTime += interval;
            yield return new WaitForSeconds(interval);
        }

        foreach (GameObject star in stars)
            star.SetActive(true);
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
        nextRoundButton.onClick.AddListener(NextRoundButtonClicked);
    }

    void NextRoundButtonClicked()
    {
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

        foreach (var obj in stars)
        {
            Destroy(obj);
        }
        stars.Clear();

        Destroy(clock);
        Destroy(dice);

        nextRoundButton.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);

        if(GetRoundNumber() == 8)
        {
            roundText = GameObject.Find("RuleText").GetComponent<TMP_Text>();
            roundText.text = "";
        }
    }
}
