using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round7 : Sentence
{
    int currentRound;
    public Sprite backGround;
    public GameObject sentencePrefab;
    public GameObject sentenceBack;
    public GameObject round7clock;
    public GameObject round7dice;

    List<string> round7Sentences = new List<string>();

    protected override void SetUpRound()
    {
        currentRound = 7;

        GameManager gameManager = FindObjectOfType<GameManager>();
        round7Sentences = gameManager.getRoundSentence(currentRound);
    }

    protected override int GetRoundNumber()
    {
        return currentRound;
    }

    protected override List<string> GetSentences()
    {
        return round7Sentences;
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
        return round7clock;
    }

    protected override GameObject GetSentenceBackGroundPrefab()
    {
        return sentenceBack;
    }

    protected override GameObject GetDice()
    {
        return round7dice;
    }
}
