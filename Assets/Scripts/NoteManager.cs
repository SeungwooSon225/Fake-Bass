using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public GameManager GameManager;
    public List<GameObject> NoteLane = new List<GameObject>();
    public StartPoint StartPoint;
    public HitBar HitBar;
    public float Scale = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.IsGamePlaying) return;

        for (int i = 0; i < NoteLane.Count; i++)
        {
            NoteLane[i].GetComponentInChildren<StartPoint>().MoveStartPointToVideoSpeed();
            NoteLane[i].GetComponentInChildren<HitBar>().PressHitBar();
        }
    }
}
