using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    // -2:게임 방법, 0:오프닝

    public int current_round = 0;

    public List<GameObject> roundPrefabs = new List<GameObject>();

    public GameObject rulePrefab;

    AudioSource bgm;
    public AudioClip playClip;

    List<GameObject> roundObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        changeRound();
        bgm = GameObject.Find("Audio Source").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        changeRound();
    }

    void changeRound()
    {
        if (current_round == 0)
            StartOpening();
        else if (current_round == -2)
            StartRule();
        else if (current_round == 1)
            StartRound1();
    }

    public void changeCurrentRound(int roundIndex)
    {
        DestroyCurrentObject();
        if (roundIndex < roundPrefabs.Count)
        {
            roundObj.Add(Instantiate(roundPrefabs[roundIndex],
                transform.position, Quaternion.identity));
            current_round = roundIndex;
        }
    }

    void DestroyCurrentObject()
    {
        if(roundObj.Count > 0)
        {
            Destroy(roundObj[0]);
            roundObj.RemoveAt(0);
        }
    }

    void StartOpening()
    {
        DestroyCurrentObject();
        roundObj.Add(Instantiate(roundPrefabs[0], transform.position, Quaternion.identity));
        current_round = -1;
    }

    void StartRule()
    {
        DestroyCurrentObject();
        roundObj.Add(Instantiate(rulePrefab, transform.position, Quaternion.identity));
        current_round = -1;
    }

    void StartRound1()
    {
        DestroyCurrentObject();
        roundObj.Add(Instantiate(roundPrefabs[1], transform.position, Quaternion.identity));
        current_round = -1;

        bgm.clip = playClip;
        bgm.Play();
    }

}
