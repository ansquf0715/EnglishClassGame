using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class teacher : MonoBehaviour
{
    TextMeshProUGUI currentTime;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        GameObject canvas = GameObject.Find("Canvas");

        Transform currentTimeTransform = this.gameObject.transform.Find("CurrentTime");
        currentTime = currentTimeTransform.GetComponent<TextMeshProUGUI>();
        currentTime.text = gameManager.time.ToString();
    }

    private void OnEnable()
    {
        currentWordsOutput();
    }

    void currentWordsOutput()
    {
        //gameManager.getAllWords();
        Debug.Log(gameManager.getAllWords());
    }

}
