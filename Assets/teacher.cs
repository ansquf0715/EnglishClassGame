using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class teacher : MonoBehaviour
{
    TextMeshProUGUI currentTime;
    TextMeshProUGUI currentWords;
    TextMeshProUGUI currentSentences;

    GameManager gameManager;

    List<string> words;
    List<string> sentences;

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

        Transform currentSentencesTransform = this.gameObject.transform.Find("SentenceOutput");
        currentSentences = currentSentencesTransform.GetComponent<TextMeshProUGUI>();

    }

    private void OnEnable()
    {
        currentWordsOutput();
        currentSentencesOutput();
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

    void currentSentencesOutput()
    {
        sentences = gameManager.getAllSentences();
        for(int i=0; i<sentences.Count; i++)
        {
            Debug.Log(sentences[i]);
            currentSentences.text += sentences[i] + "\n";
        }
    }

}
