using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Sentence : MonoBehaviour
{
    protected TMP_Text roundText;
    protected List<string> sentences = new List<string>();

    public int time;
    protected GameObject timer;
    protected Slider slider;
    protected GameObject clock;

    Button showButton;
    Button nextRoundButton;

    public List<GameObject> boxObjects = new List<GameObject>();
    public List<GameObject> textObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
