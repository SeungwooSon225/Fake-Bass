using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTempo : MonoBehaviour
{
    public MidiPlayerTK.MidiFilePlayer MidiFilePlayer;
    public float TargetTempo;
    public bool IsTempoAdjustable = false;

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
        float originalTempo = (float)MidiFilePlayer.MPTK_Tempo;

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
                float noddingTerm = turningTime - previousTime;

                int temporaryTempo = (int)(60 / noddingTerm);
                
                Debug.Log(temporaryTempo);

                if (temporaryTempo > 30) TargetTempo = temporaryTempo;

                previousTime = Time.time;
            }

            previousTurningPoint = turnigPoint;            
        }

        previousAngle = adjustedAngle;
        previousAngularVelocity = angularVelocity;
    }
}
