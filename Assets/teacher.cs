using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class teacher : MonoBehaviour
{
    TextMeshProUGUI currentTime;
    TextMeshProUGUI currentWords;
    TextMeshProUGUI currentSentences;
    TextMeshProUGUI newWords;

    InputField newWordInputField;

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

        Transform newWordTransform = this.gameObject.transform.Find("NewWord");
        newWords = newWordTransform.GetComponent<TextMeshProUGUI>();

        Transform wordInputTransform = this.gameObject.transform.Find("WordInput");
        newWordInputField = wordInputTransform.GetComponent<InputField>();
        Debug.Log(newWordInputField);
    }

    private void OnEnable()
    {
        currentWordsOutput();
        currentSentencesOutput();
        newWordInputField.onEndEdit.AddListener(SubmitWord);
    }

    void currentWordsOutput()
    {
        words = gameManager.getAllWords();
        for(int i=0; i<words.Count; i++)
        {
            currentWords.text += words[i] + "\n";
        }

    }

    void currentSentencesOutput()
    {
        sentences = gameManager.getAllSentences();
        for(int i=0; i<sentences.Count; i++)
        {
            currentSentences.text += sentences[i] + "\n";
        }
    }

    void SubmitWord(string word)
    {
        Debug.Log("여기되나");
        if(Input.GetKeyDown(KeyCode.Return))
        {
            newWords.text = word;
            newWordInputField.text = "";
        }
    }
}
