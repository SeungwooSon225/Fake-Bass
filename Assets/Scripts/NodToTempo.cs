using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodToTempo : MonoBehaviour
{
    public RealisticEyeMovements.EyeAndHeadAnimator EyeAndHeadAnimator;
    public MusicDataReader MusicDataReader;
    public VideoManager VideoManager;
    //public MidiPlayerTK.MidiFilePlayer MidiFilePlayer;
    public Animator GuitaristAnimator;
    public ControlTempo ControlTempo;

    public bool isStart = false;
    public float speed = 1.0f;
    public float GlobalTempo;

    [SerializeField]
    private int minimumTempoChange = 10;

    private bool isNodding = false;
    [SerializeField]
    private float noddingTerm;
    private int noddingFrame = 20;
    private float maxNodding = 30f;
    private bool isTempo = false;

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (!ControlTempo.IsTempoAdjustable || !GuitaristAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayingGuitar")) return;
        float animationState = GuitaristAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        animationState -= (int)animationState;

        if ((animationState > 0.95f && animationState < 1.0f) || (animationState > 0.45f && animationState < 0.5f))
        {
            if (!isTempo) return;

            isTempo = false;

            if (Mathf.Abs(GlobalTempo - ControlTempo.TargetTempo) < minimumTempoChange)
            {
                GlobalTempo = ControlTempo.TargetTempo;
            }
            else
            {
                GlobalTempo = (GlobalTempo > ControlTempo.TargetTempo) ? GlobalTempo - minimumTempoChange : GlobalTempo + minimumTempoChange;
            }

            //VideoManager.Video.playbackSpeed = GlobalTempo / MusicDataReader.MusicData.Tempo;
            VideoManager.ChangeVideoSpeed(GlobalTempo / MusicDataReader.MusicData.Tempo);

            GuitaristAnimator.speed = (MusicDataReader.MusicData.Tempo * VideoManager.Video.playbackSpeed) / 120f;
        }
        else
        {
            isTempo = true;
        }
    }
}
