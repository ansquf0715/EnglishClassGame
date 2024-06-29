using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    // -2:게임 방법, 0:오프닝

    public int current_round = 0;

    public List<GameObject> roundPrefabs = new List<GameObject>();
    //public GameObject openingPrefab;
    //public GameObject round1Prefab;

    List<GameObject> roundObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (current_round == 0)
            StartOpening();
        else if (current_round == 1)
            StartRound1();
    }

    // Update is called once per frame
    void Update()
    {
        if (current_round == 0)
            StartOpening();
        else if (current_round == 1)
            StartRound1();
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
        //if (openingPrefab != null)
        //{
        //    //Instantiate(openingPrefab, transform.position, Quaternion.identity);
        //    roundObj.Add(Instantiate(openingPrefab, transform.position, Quaternion.identity));
        //    current_round = -1;
        //}
        roundObj.Add(Instantiate(roundPrefabs[0], transform.position, Quaternion.identity));
        current_round = -1;
    }

    void StartRound1()
    {
        DestroyCurrentObject();
        //if (round1Prefab != null)
        //{
        //    roundObj.Add(Instantiate(round1Prefab, transform.position, Quaternion.identity));
        //    //Instantiate(round1Prefab, transform.position, Quaternion.identity);
        //    current_round = -1;
        //}
        roundObj.Add(Instantiate(roundPrefabs[1], transform.position, Quaternion.identity));
        current_round = -1;
    }
}
