using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTempo : MonoBehaviour
{
    //public MidiPlayerTK.MidiFilePlayer MidiFilePlayer;
    public MusicDataReader MusicDataReader;
    public float TargetTempo;
    public bool IsTempoAdjustable = false;
    public float noddingTerm;

    private float previousAngle;
    private float previousAngularVelocity;
    private float previousTurningPoint;
    private float previousTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTempoAdjustable) return;
        float adjustedAngle;
        float originalTempo = MusicDataReader.MusicData.Tempo;

        if (gameObject.transform.eulerAngles.x > 200f)
        {
            adjustedAngle = 360f - gameObject.transform.eulerAngles.x;
        }
        else
        {
            adjustedAngle = -gameObject.transform.eulerAngles.x;
        }
        
        float angularVelocity = adjustedAngle - previousAngle;

        if (angularVelocity * previousAngularVelocity < 0)
        {
            float turnigPoint = adjustedAngle;

            if (Mathf.Abs(turnigPoint - previousTurningPoint) > 10f && angularVelocity > 0)
            {
                float turningTime = Time.time;
                noddingTerm = turningTime - previousTime;

                // Control tempo by tempo
                int temporaryTempo = (int)(60 / noddingTerm);

                if (temporaryTempo > 30)
                {
                    Debug.Log("t: " + temporaryTempo);
                    TargetTempo = temporaryTempo;
                }

                // Control tempo by beat



                previousTime = Time.time;
            }

            previousTurningPoint = turnigPoint;            
        }

        previousAngle = adjustedAngle;
        previousAngularVelocity = angularVelocity;
    }
}
