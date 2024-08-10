using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round3 : Round
{
    int currentRound;
    public Sprite backGround;
    public GameObject wordPrefab;
    public GameObject wordBack;
    public GameObject round3clock;

    List<string> round3words = new List<string>();

    protected override void SetUpRound()
    {
        currentRound = 3;

        GameManager gameManager = FindObjectOfType<GameManager>();
        round3words = gameManager.getRoundWords(currentRound);
    }

    protected override int GetRoundNumber()
    {
        return currentRound;
    }

    protected override List<string> GetWords()
    {
        return round3words;
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
        return round3clock;
    }

    protected override GameObject GetWordBackGroundPrefab()
    {
        return wordBack;
    }
}