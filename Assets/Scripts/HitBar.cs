using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBar : MonoBehaviour
{
    public GameManager GameManager;
    public NotePoolManager NotePoolManager;
    public VideoManager VideoManager;
    public VRControllerInputManager VRControllerInputManager;
    public GuitarInteractionManager GuitarInteractionManager;
    public SoundManager SoundManager;
    public Animator GuitaristAnimator;
    public RPCManager RPCManager;

    public ParticleSystem RippleEffect;

    private bool isNoteInHitBar = false;
    private GameObject currentNote;

    [SerializeField]
    private float previousPitch;
    [SerializeField]
    private float currentPitch;
    [SerializeField]
    private int currentNoteIndexInNotePool;
    [SerializeField]
    private bool isPitchChanged;
    [SerializeField]
    private bool isHandMove = false;
    private bool isMissNote = false;
    private float animationSpeed;


    public void InitializeNoteInfo()
    { 
        currentNoteIndexInNotePool = 0;
        currentPitch = NotePoolManager.NotePool[currentNoteIndexInNotePool].GetComponent<Note>().pitch;
        previousPitch = currentPitch;
        isPitchChanged = false;

        gameObject.GetComponent<Collider>().enabled = true;
    }


    public void PressHitBar()
    {
        //// Check hand move or not
        //if (Input.GetKeyDown(KeyCode.LeftArrow)) isHandMove = false;
        //if (Input.GetKeyDown(KeyCode.RightArrow)) isHandMove = true;

        if (VRControllerInputManager.RightPressed() || Input.GetKeyDown(KeyCode.Space))
        {
            RippleEffect.Play();

            isHandMove = GuitarInteractionManager.CheckChordChange();

            switch (GameManager.CurrentGameLevel)
            {
                case GameManager.GameLevel.None:
                    Debug.Log("Good");
                    SoundManager.Play(currentPitch);
                    RPCManager.MakeSound(currentPitch);

                    if (currentNote != null)
                    {
                        NotePoolManager.SetNoteOnProperPosition(currentNote);
                        UpdateNoteInfo();

                        currentNote = null;

                        if (VideoManager.Video.isPaused)
                        {
                            VideoManager.Video.Play();

                            GuitaristAnimator.speed = animationSpeed;
                        }
                    }

                    break;

                case GameManager.GameLevel.Hand:

                    if (!isMissNote && (isPitchChanged ^ isHandMove))
                    {
                        // Make wrong sound
                        Debug.Log("Miss");
                        SoundManager.Play(currentPitch + 3);
                    }
                    // Pitch changed and hand move or pitch unchanged and hand not move, or miss previous note
                    else
                    {
                        // Make proper sound
                        Debug.Log("Good");
                        SoundManager.Play(currentPitch);
                    }

                    isPitchChanged = false;

                    if (currentNote != null)
                    {
                        NotePoolManager.SetNoteOnProperPosition(currentNote);
                        UpdateNoteInfo();

                        currentNote = null;

                        if (VideoManager.Video.isPaused)
                        {
                            VideoManager.Video.Play();
                            GuitaristAnimator.speed = animationSpeed;
                        }
                    }

                    isMissNote = false;

                    break;

                case GameManager.GameLevel.Timing:
                    if (currentNote != null)
                    {
                        Debug.Log("Good");
                        SoundManager.Play(currentPitch);

                        NotePoolManager.SetNoteOnProperPosition(currentNote);
                        UpdateNoteInfo();

                        currentNote = null;

                        if (VideoManager.Video.isPaused)
                        {
                            VideoManager.Video.Play();
                            GuitaristAnimator.speed = animationSpeed;
                        }
                    }
                    break;

                case GameManager.GameLevel.Both:
                    if (currentNote != null)
                    {
                        // Pitch changed but hand not move or pitch unchanged but hand move
                        if (!isMissNote && (isPitchChanged ^ isHandMove))
                        {
                            // Make wrong sound
                            Debug.Log("Miss");
                            SoundManager.Play(currentPitch + 3);
                        }
                        // Pitch changed and hand move or pitch unchanged and hand not move, or miss previous note
                        else
                        {
                            // Make proper sound
                            Debug.Log("Good");
                            SoundManager.Play(currentPitch);
                        }

                        NotePoolManager.SetNoteOnProperPosition(currentNote);
                        UpdateNoteInfo();

                        currentNote = null;

                        if (VideoManager.Video.isPaused)
                        {
                            VideoManager.Video.Play();
                            GuitaristAnimator.speed = animationSpeed;
                        }
                    }

                    isMissNote = false;

                    break;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Note")
        {
            currentNote = other.gameObject;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (currentNote != null && other.gameObject.tag == "Note")
        {
            // If Basic mode, then just set note on next position
            if (GameManager.CurrentGameMode == GameManager.GameMode.Basic)
            {
                NotePoolManager.SetNoteOnProperPosition(currentNote);
                UpdateNoteInfo();

                isMissNote = true;
                currentNote = null;
            }
            // If Fake Play mode, then waiting input
            else if (GameManager.CurrentGameMode == GameManager.GameMode.FakePlay)
            { 
                VideoManager.Video.Pause();
                animationSpeed = GuitaristAnimator.speed;
                GuitaristAnimator.speed = 0;
            }
        }
    }


    private void UpdateNoteInfo()
    {
        currentNoteIndexInNotePool++;
        
        if (currentNoteIndexInNotePool == NotePoolManager.NotePool.Count)
        {
            currentNoteIndexInNotePool = 0;
        }

        previousPitch = currentPitch;
        currentPitch = NotePoolManager.NotePool[currentNoteIndexInNotePool].GetComponent<Note>().pitch;

        if (previousPitch != currentPitch)
        {
            isPitchChanged = true;
        }
        else 
        {
            isPitchChanged = false;
        }
    }
}
