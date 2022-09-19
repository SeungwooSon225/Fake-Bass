using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public MidiPlayerTK.MidiFilePlayer MidiFilePlayer;
    public ControlTempo ControlTempo;
    public NodToTempo NodToTempo;
    public Animator GuitaristAnimator;
    public NotePoolManager NotePoolManager;
    public MusicDataReader MusicDataReader;
    public HitBar HitBar;
    public VRControllerInputManager VRControllerInputManager;
    public RPCManager RPCManager;

    public GameMode CurrentGameMode;
    public GameLevel CurrentGameLevel;
    public Instrumnets CurrentInstrument;

    public GameObject[] GameObjectsForBass;
    public GameObject[] GameObjectsForDrum;

    public bool IsGameEnd = false;
    public bool IsGamePlaying = false;
    private int isGuitarPlayingHash;

    private static GameManager _instance;

    public Components ready;

    [SerializeField]
    private VideoManager videoManager;


    public enum GameMode
    {
        Basic,
        FakePlay
    }


    public enum GameLevel
    {
        None,
        Timing,
        Hand,
        Both
    }


    public enum Instrumnets
    {
        Bass,
        Drum
    }


    public static GameManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        isGuitarPlayingHash = Animator.StringToHash("isGuitarPlaying");

        StartCoroutine(GameLoop());
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator GameLoop()
    {
        // RoundStarting �ڷ�ƾ�� �۵����� �� ���� �÷��̿� �ʿ��� ���� �ε�, RoundStarting �ڷ�ƾ�� �Ϸ�� �� return �ȴ�.
        yield return StartCoroutine(RoundSetting());

        // RoundStarting �ڷ�ƾ�� �Ϸ�Ǹ� RoundPlaying �ڷ�ƾ�� �۵����� ���� �÷���, �� ���尡 ����� �� return �ȴ�.
        yield return StartCoroutine(RoundPlaying());

        // RoundPlaying �ڷ�ƾ�� �Ϸ�Ǹ� RoundEnding �ڷ�ƾ�� �۵����� �� ���带 ������, �ʿ��� �۾��� ��ģ �� return �ȴ�.
        yield return StartCoroutine(RoundEnding());

        // ������ ������ �������� ���� ����
        if (IsGameEnd)
        {
            Debug.Log("Game End");
        }
        // ������ ������ ������ �ƴϸ� ���� ����(��) ����
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    /* 
     * �� ���带 �÷��� �ϴ� �� �ʿ��� ������ �ε��ϴ� �ڷ�ƾ
     */
    private IEnumerator RoundSetting()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Round Setting...");

        /* To do
         * ���� �÷��̸� �� ������ ������ �ܰ谡 �߰��� ����
         */

        switch (CurrentInstrument)
        {
            case Instrumnets.Bass:
                foreach (GameObject gameObjectForBass in GameObjectsForBass)
                {
                    gameObjectForBass.SetActive(true);
                }

                foreach (GameObject gameObjectForDrum in GameObjectsForDrum)
                {
                    gameObjectForDrum.SetActive(false);
                }

                NotePoolManager.MakeNoteQueue(MusicDataReader.AdjustedNoteInfoArray);
                NotePoolManager.InitializeNotes();
                HitBar.InitializeNoteInfo();
                break;

            case Instrumnets.Drum:
                foreach (GameObject gameObjectForBass in GameObjectsForBass)
                {
                    gameObjectForBass.SetActive(false);
                }

                foreach (GameObject gameObjectForDrum in GameObjectsForDrum)
                {
                    gameObjectForDrum.SetActive(true);
                }

                break;

            default:
                break;
        }


        Debug.Log("Round Setting Done");

        yield return new WaitForSeconds(0.5f); 
    }


    /* 
    * �� ���带 �÷����ϴ� �ڷ�ƾ
    */
    private IEnumerator RoundPlaying()
    {
        Debug.Log("Round Playing...");

        bool isKeyDown = false;
        while (!isKeyDown)
        {
            // Ű���� �Է��� ������ �÷��� ����
            if (Input.anyKeyDown || VRControllerInputManager.RightPressed())
            {
                RPCManager.StartMusic();
            }
            // 
            if (ready.myStart && ready.otherStart)
            {
                // TO do
                // ���� �Է� ���

                ///////////////////////
                ///
                isKeyDown = true;
                ready.myStart = false;
                ready.otherStart = false;

                videoManager.Video.Play();
                IsGamePlaying = true;

                /* =========================================
                 * ���� ���� ���;׼�
                 */
                //MidiFilePlayer.MPTK_RePlay();
                //NodToTempo.isStart = true;
                ControlTempo.TargetTempo = MusicDataReader.MusicData.Tempo;
                Debug.Log("C: "+ ControlTempo.TargetTempo);
                //NodToTempo.GlobalTempo = MusicDataReader.MusicData.Tempo;
                // =========================================

                GuitaristAnimator.SetBool(isGuitarPlayingHash, true);
            }

            // Ű���� �Է��� ������ ���� �����ӿ� �� ��ġ�� �ٽ� ���ƿ´�.
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        // ������ ������ ���� �ܰ�� �Ѿ��.
        while (videoManager.Video.time < videoManager.Video.length)
        //while(MidiFilePlayer.MPTK_IsPlaying)
        {
            yield return null;
        }

        Debug.Log("Round Playing Done");
        IsGamePlaying = false;

        yield return new WaitForSeconds(1.0f);
    }


    /* 
     * �� ���带 ������ �ڷ�ƾ
     */
    private IEnumerator RoundEnding()
    {
        Debug.Log("Round Ending...");

        bool isKeyDown = false;

        while (!isKeyDown)
        {
            // ESC�� ������ ���� ������ ����
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isKeyDown = true;
                IsGameEnd = true;
            }
            // ESC�� ������ Ű�� ������ ���� ���� ����
            else if (Input.anyKeyDown)
            {
                isKeyDown = true;
            }

            yield return null;
        }

        Debug.Log("Round Ending Done");
    }
}
