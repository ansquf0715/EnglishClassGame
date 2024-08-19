using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round6 : Sentence
{
    int currentRound;
    public Sprite backGround;
    public GameObject sentencePrefab;
    public GameObject sentenceBack;
    public GameObject round5clock;
    public GameObject round5dice;

    List<string> round6Sentences = new List<string>();

    protected override void SetUpRound()
    {
        currentRound = 6;

        GameManager gameManager = FindObjectOfType<GameManager>();
        round6Sentences = gameManager.getRoundSentence(currentRound);
    }

    protected override int GetRoundNumber()
    {
        return currentRound;
    }

    protected override List<string> GetSentences()
    {
        return round6Sentences;
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
        return round5clock;
    }

    protected override GameObject GetSentenceBackGroundPrefab()
    {
        return sentenceBack;
    }

    protected override GameObject GetDice()
    {
        return round5dice;
    }
}
