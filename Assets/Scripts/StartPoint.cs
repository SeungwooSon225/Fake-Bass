using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public NoteManager NoteManager;
    public VideoManager VideoManager;


    public void MoveStartPointToVideoSpeed()
    { 
        gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, (float)VideoManager.Video.time * NoteManager.Scale);
    }
}
