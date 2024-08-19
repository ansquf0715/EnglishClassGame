using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round8 : Sentence
{
    int currentRound;
    public Sprite backGround;
    public GameObject sentencePrefab;
    public GameObject sentenceBack;
    public GameObject round8clock;
    public GameObject round8dice;

    List<string> round8Sentences = new List<string>();

    protected override void SetUpRound()
    {
        currentRound = 8;

        GameManager gameManager = FindObjectOfType<GameManager>();
        round8Sentences = gameManager.getRoundSentence(currentRound);
    }

    protected override int GetRoundNumber()
    {
        return currentRound;
    }

    protected override List<string> GetSentences()
    {
        return round8Sentences;
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
        return round8clock;
    }

    protected override GameObject GetSentenceBackGroundPrefab()
    {
        return sentenceBack;
    }

    protected override GameObject GetDice()
    {
        return round8dice;
    }
}
