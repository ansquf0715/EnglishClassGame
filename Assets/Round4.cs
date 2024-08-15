using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round4: Sentence
{
    int currentRound;
    public Sprite backGround;
    public GameObject sentencePrefab;
    public GameObject sentenceBack;
    public GameObject round4clock;

    List<string> round4Sentences = new List<string>();

    protected override void SetUpRound()
    {
        currentRound = 4;

        GameManager gameManager = FindObjectOfType<GameManager>();
        round4Sentences = gameManager.getRoundSentence(currentRound);
    }

    protected override int GetRoundNumber()
    {
        return currentRound;
    }

    protected override List<string> GetSentences()
    {
        return round4Sentences;
    }

    protected override Sprite GetBackGround()
    {
        return backGround;
    }

    protected override GameObject GetSentencePrefab()
    {
        return sentencePrefab;
    }

    protected override GameObject GetClock()
    {
        return round4clock;
    }

    protected override GameObject GetSentenceBackGroundPrefab()
    {
        return sentenceBack;
    }
}