using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<string> wordsList = new List<string>();
    Dictionary<int, List<string>> wordsForRounds = new Dictionary<int, List<string>>();

    List<string> sentencesList = new List<string>();
    Dictionary<int, List<string>> sentencesForRounds = new Dictionary<int, List<string>>(); 

    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> wordData = CSVReader.Read("words");

        //Debug.Log("Test CSV Reader : " + data.Count);

        foreach(var row in wordData)
        {
            if (row.ContainsKey("words"))
                wordsList.Add(row["words"].ToString());
        }

        List<Dictionary<string, object>> sData = CSVReader.Read("sentences");

        foreach (var row in sData)
        {
            if (row.ContainsKey("sentence"))
                sentencesList.Add(row["sentence"].ToString());
        }


        //Debug.Log(" sentence count" + sentencesList.Count);
        //foreach (var a in sentencesList)
        //    Debug.Log(a);

        //Initialize the rounds dictionary
        for (int i = 1; i <= 3; i++)
            wordsForRounds[i] = new List<string>();

        distributeWordsIntoRounds();

        for (int i=4; i<=8; i++)
        {
            sentencesForRounds[i] = new List<string>();
        }

        distributeSentencesIntoRounds();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<string> getRoundWords(int round)
    {
        //return new List<string>(wordsForRounds[round]);
        if(wordsForRounds.ContainsKey(round))
        {
            return new List<string>(wordsForRounds[round]);
        }
        else
        {
            return new List<string>();
        }
    }

    void distributeWordsIntoRounds()
    {
        int roundIndex = 1;
        HashSet<string> usedWords = new HashSet<string>();

        while(usedWords.Count < wordsList.Count)
        {
            foreach(string word in wordsList)
            {
                if(!usedWords.Contains(word)
                    && wordsForRounds[roundIndex].Count < 9)
                {
                    wordsForRounds[roundIndex].Add(word);
                    usedWords.Add(word);
                    roundIndex = roundIndex % 3 + 1;

                    if (wordsForRounds[1].Count >= 9
                        && wordsForRounds[2].Count >= 9
                        && wordsForRounds[3].Count >= 9)
                        return;
                }
            }
        }

        while(wordsForRounds[1].Count < 9
            || wordsForRounds[2].Count < 9
            || wordsForRounds[3].Count <9)
        {
            foreach(string word in wordsList)
            {
                if(wordsForRounds[roundIndex].Count < 9
                    && !wordsForRounds[roundIndex].Contains(word))
                {
                    wordsForRounds[roundIndex].Add(word);

                    if (wordsForRounds[1].Count >= 9 
                        && wordsForRounds[2].Count >= 9 
                        && wordsForRounds[3].Count >= 9)
                    {
                        break;
                    }
                }
                roundIndex = roundIndex % 3 + 1;
            }
        }
    }
    
    void distributeSentencesIntoRounds()
    {
        int sentencesPerRound = 3;
        int totalRound = 5;
        int totalSentencesNeed = sentencesPerRound * totalRound;

        for(int roundIndex = 4; roundIndex <= 8; roundIndex++)
        {
            for(int i=0; i<sentencesPerRound; i++)
            {
                int sentenceIndex = (roundIndex - 4) * sentencesPerRound + i;
                if(sentenceIndex < sentencesList.Count)
                {
                    sentencesForRounds[roundIndex].Add(sentencesList[sentenceIndex]);
                }
                else
                {
                    int randomIndex = Random.Range(0, sentencesList.Count);
                    sentencesForRounds[roundIndex].Add(sentencesList[randomIndex]);
                }
            }
        }

        // 각 라운드에 들어있는 문장 출력
        //for (int i = 4; i <= 8; i++)
        //{
        //    Debug.Log($"Round {i} sentences: {string.Join(", ", sentencesForRounds[i])}");
        //}
    }

    void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while(n>1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
