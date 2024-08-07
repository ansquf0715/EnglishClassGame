
using System;
using System.Collections.Generic;
using UnityEngine;

public class Round1 : Round
{
    int currentRound;
    public Sprite backGround;
    public GameObject wordPrefab;
    public GameObject wordBack;
    public GameObject round1clock;

    List<string> round1words = new List<string>();

    protected override void SetUpRound()
    {
        currentRound = 1;

        GameManager gameManager = FindObjectOfType<GameManager>();
        round1words = gameManager.getRoundWords(currentRound);
    }

    protected override int GetRoundNumber()
    {
        return currentRound;
    }

    protected override List<string> GetWords()
    {
        return round1words;
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
        return round1clock;
    }

    protected override GameObject GetWordBackGroundPrefab()
    {
        return wordBack;
    }
}