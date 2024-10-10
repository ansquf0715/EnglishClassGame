using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class teacher : MonoBehaviour
{
    TextMeshProUGUI currentTime;
    TextMeshProUGUI currentWords;

    GameManager gameManager;

    List<string> words;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        GameObject canvas = GameObject.Find("Canvas");

        Transform currentTimeTransform = this.gameObject.transform.Find("CurrentTime");
        currentTime = currentTimeTransform.GetComponent<TextMeshProUGUI>();
        currentTime.text = gameManager.time.ToString();

        Transform currentWordsTransform = this.gameObject.transform.Find("WordOutput");
        currentWords = currentWordsTransform.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        currentWordsOutput();
    }

    void currentWordsOutput()
    {
        words = gameManager.getAllWords();
        for(int i=0; i<words.Count; i++)
        {
            Debug.Log(words[i]);
            currentWords.text += words[i] + "\n";
        }

    }

}
