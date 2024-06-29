using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Opening : MonoBehaviour
{
    public GameObject waterPerson;
    public GameObject guide;
    public GameObject raccoon1;
    public GameObject raccoon2;
    public GameObject raccoon3;
    public GameObject logo;
    public GameObject appleMan;
    public GameObject blackMan;

    TMP_Text wordLottoText;

    List<GameObject> instantiatedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Opening object started");

        instantiatedObjects.Add(Instantiate(waterPerson, new Vector2(-2.4f, 0.4f), Quaternion.identity));
        instantiatedObjects.Add(Instantiate(guide, new Vector2(-5.6f, -1f), Quaternion.identity));
        instantiatedObjects.Add(Instantiate(raccoon1, new Vector2(-1.8f, -1f), Quaternion.identity));
        instantiatedObjects.Add(Instantiate(raccoon2, new Vector2(1.6f, -1f), Quaternion.identity));
        instantiatedObjects.Add(Instantiate(raccoon3, new Vector2(5.8f, -1.1f), Quaternion.identity));

        //Instantiate(waterPerson, new Vector2(-2.4f, 0.4f), Quaternion.identity);
        //Instantiate(guide, new Vector2(-5.6f, -1f), Quaternion.identity);
        //Instantiate(raccoon1, new Vector2(-1.8f, -1f), Quaternion.identity);
        //Instantiate(raccoon2, new Vector2(1.6f, -1f), Quaternion.identity);
        //Instantiate(raccoon3, new Vector2(5.8f, -1.1f), Quaternion.identity);

        // create & move apple man
        GameObject appleManInstance = Instantiate(appleMan, new Vector2(-10f, 0f), Quaternion.identity);
        instantiatedObjects.Add(appleManInstance);
        StartCoroutine(MoveObject(appleManInstance, new Vector2(-7.5f, 0f), 2f));

        // create & move black man
        GameObject blackManInstance = Instantiate(blackMan, new Vector2(10f, 0f), Quaternion.identity);
        instantiatedObjects.Add(blackManInstance);
        StartCoroutine(MoveObject(blackManInstance, new Vector2(7.5f, 0f), 2f));

        GameObject logoInstance = Instantiate(logo, new Vector2(0f, 7.5f), Quaternion.identity);
        instantiatedObjects.Add(logoInstance);
        StartCoroutine(MoveObject(logoInstance, new Vector2(0f, 2.5f), 2f));

        //Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        wordLottoText = GameObject.Find("WordLotto").GetComponent<TMP_Text>();
        wordLottoText.text = "";
        StartCoroutine(TypeText(wordLottoText, "WORD LOTTO!"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveObject(GameObject obj, Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = obj.transform.position;
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            obj.transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition;
    }

    IEnumerator TypeText(TMP_Text textObj,string textToType)
    {
        yield return new WaitForSeconds(1f);

        string displayText = textToType;
        //textObj.text = "";
        foreach(char c in displayText)
        {
            textObj.text += c;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void PlayButtonClicked()
    {
        Debug.Log("Play button clicked");
        RoundManager roundManager = FindObjectOfType<RoundManager>();
        if (roundManager != null)
            roundManager.current_round = 1;
    }

    private void OnDestroy()
    {
        foreach(GameObject obj in instantiatedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        instantiatedObjects.Clear();
    }
}
