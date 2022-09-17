using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZeroScore : MonoBehaviour
{
    ScoreSystem allScore;

    [SerializeField]
    Slider slider;
    [SerializeField]
    GameObject fillArea;
    [SerializeField]
    private VideoManager videoManager;

    // Start is called before the first frame update
    void Start()
    {
        allScore = GameObject.Find("GlobalScore").GetComponent<ScoreSystem>();
        slider.value = allScore.score;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = allScore.score;
        if (slider.value <= 0)
        {
            fillArea.SetActive(false);
            videoManager.Video.Pause();
        }
        else
            fillArea.SetActive(true);
    }
}
