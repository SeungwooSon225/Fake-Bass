using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer Video;

    public float VideoSpeed = 1.0f;


    public void ChangeVideoSpeed()
    {
        Video.playbackSpeed = VideoSpeed;
    }


    private void Update()
    {
        Video.playbackSpeed = VideoSpeed;
    }
}
