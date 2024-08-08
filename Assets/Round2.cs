using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round2 : Round
{
    int currentRound;
    public Sprite backGround;
    public GameObject wordPrefab;
    public GameObject wordBack;
    public GameObject round2clock;

    List<string> round2words = new List<string>();

    protected override void SetUpRound()
    {
        currentRound = 2;

        GameManager gameManager = FindObjectOfType<GameManager>();
        round2words = gameManager.getRoundWords(currentRound);
    }

    protected override int GetRoundNumber()
    {
        return currentRound;
    }

    protected override List<string> GetWords()
    {
        return round2words;
    }

    protected override Sprite GetBackGround()
    {
        return backGround;
    }

    protected override GameObject GetWordPrefab()
    {
        return wordPrefab;
    }

    protected override GameObject GetClock()
    {
        return round2clock;
    }

    protected override GameObject GetWordBackGroundPrefab()
    {
        return wordBack;
    }
}
