using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeTempo : MonoBehaviour
{
    public ControlTempo ControlTempo;
    public MidiPlayerTK.MidiFilePlayer MidiFilePlayer;

    private float GlobalTempo;

    [SerializeField]
    private int minimumTempoChange = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!ControlTempo.IsTempoAdjustable) return;

        //if (Mathf.Abs(GlobalTempo - ControlTempo.TargetTempo) < minimumTempoChange)
        //{
        //    GlobalTempo = ControlTempo.TargetTempo;
        //}
        //else 
        //{
        //    GlobalTempo = (GlobalTempo > ControlTempo.TargetTempo) ? GlobalTempo - minimumTempoChange : GlobalTempo + minimumTempoChange;
        //}

        //MidiFilePlayer.MPTK_Speed = GlobalTempo / (float)MidiFilePlayer.MPTK_Tempo;
    }
}
