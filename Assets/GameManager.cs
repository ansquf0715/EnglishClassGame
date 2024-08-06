using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<string> wordsList = new List<string>();
    Dictionary<int, List<string>> wordsForRounds = new Dictionary<int, List<string>>();

    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("words");

        //Debug.Log("Test CSV Reader : " + data.Count);

        foreach(var row in data)
        {
            if (row.ContainsKey("words"))
                wordsList.Add(row["words"].ToString());
        }

        //Initialize the rounds dictionary
        for (int i = 1; i <= 3; i++)
            wordsForRounds[i] = new List<string>();

        //Shuffle(wordsList);

        distributeWordsIntoRounds();

        //foreach(var round in wordsForRounds)
        //    Debug.Log("Round " + round.Key + " words: " + string.Join(", ", round.Value));
    
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
