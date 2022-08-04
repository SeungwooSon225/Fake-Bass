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
    private int isGuitarPlayingHash;
    private int isNoddingHash;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        isGuitarPlayingHash = Animator.StringToHash("isGuitarPlaying");
        isNoddingHash = Animator.StringToHash("isNodding");
    }


    // Update is called once per frame
    void Update()
    {
        NodToBeat();
    }


    void NodToBeat()
    {
        if (MusicDataReader.MusicData.BeatArray[index] / MusicDataReader.MusicData.Tempo * 60f < VideoManager.Video.time + 0.01f)
        {
            float oridiff = MusicDataReader.MusicData.BeatArray[index + 1] / MusicDataReader.MusicData.Tempo * 60f - MusicDataReader.MusicData.BeatArray[index] / MusicDataReader.MusicData.Tempo * 60f;
            oridiff /= VideoManager.Video.playbackSpeed;

            float diff = oridiff;

            if (ControlTempo.IsTempoAdjustable && ControlTempo.noddingTerm < 2 && ControlTempo.noddingTerm > 0.1f)
            {
                float targetDiff = ControlTempo.noddingTerm;
                Debug.Log("Target: " + targetDiff + " dd : " + diff);

                if (Mathf.Abs(diff - targetDiff) < 0.03f)
                {
                    //diff = targetDiff;
                }
                else
                {
                    diff = (diff > targetDiff) ? diff - 0.03f : diff + 0.03f;
                }

                Debug.Log("diff: " + diff + " ori : " + oridiff);
                VideoManager.ChangeVideoSpeed(VideoManager.Video.playbackSpeed * oridiff / diff);
            }

            float tempo = 60 / diff;


            //GuitaristAnimator.Rebind();
            //GuitaristAnimator.enabled = false;
            //GuitaristAnimator.enabled = true;
            //GuitaristAnimator.SetBool(isGuitarPlayingHash, true);

            GuitaristAnimator.Play("Nodding", -1, 0f);
            

            GuitaristAnimator.speed = tempo / 120f;

            index++;
        }
    }



    void NodToFixedTempo()
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
