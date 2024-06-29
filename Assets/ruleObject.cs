using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ruleObject : MonoBehaviour
{
    AudioSource bgm;
    public AudioClip audio;

    public List<GameObject> backObjects = new List<GameObject>();
    List<GameObject> instantiatedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        bgm.clip = audio;
        bgm.Play();

        instantiatedObjects.Add(Instantiate(backObjects[0],
            new Vector2(5.5f, -1), Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
