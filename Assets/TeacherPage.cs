using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeacherPage : MonoBehaviour
{
    public GameObject teachersPage;
    public TextMeshProUGUI currentTime;

    //GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        GameObject canvas = GameObject.Find("Canvas");
        teachersPage = canvas.transform.Find("Teacher'sPage").gameObject;

        Transform currentTimeTransform = teachersPage.transform.Find("CurrentTime");



        currentTime = currentTimeTransform.GetComponent<TextMeshProUGUI>();

        currentTime.text = gm.time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
