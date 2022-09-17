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
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
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
        // RoundStarting 코루틴을 작동시켜 한 라운드 플레이에 필요한 정도 로드, RoundStarting 코루틴이 완료된 후 return 된다.
        yield return StartCoroutine(RoundSetting());

        // RoundStarting 코루틴이 완료되면 RoundPlaying 코루틴을 작동시켜 게임 플레이, 한 라운드가 종료된 후 return 된다.
        yield return StartCoroutine(RoundPlaying());

        // RoundPlaying 코루틴이 완료되면 RoundEnding 코루틴을 작동시켜 한 라운드를 끝낸다, 필요한 작업을 마친 후 return 된다.
        yield return StartCoroutine(RoundEnding());

        // 게임이 완전히 끝났으면 게임 종료
        if (IsGameEnd)
        {
            Debug.Log("Game End");
        }
        // 게임이 완전히 끝난게 아니면 다음 라운드(곡) 실행
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    /* 
     * 한 라운드를 플레이 하는 데 필요한 정보를 로드하는 코루틴
     */
    private IEnumerator RoundSetting()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Round Setting...");

        /* To do
         * 게임 플레이를 할 음악을 고르는 단계가 추가될 예정
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
    * 한 라운드를 플레이하는 코루틴
    */
    private IEnumerator RoundPlaying()
    {
        Debug.Log("Round Playing...");

        bool isKeyDown = false;
        while (!isKeyDown)
        {
            // 키보드 입력이 있으면 플레이 시작
            if (Input.anyKeyDown || VRControllerInputManager.RightPressed())
            {
                RPCManager.StartMusic();
            }
            // 
            if (ready.myStart && ready.otherStart)
            {
                // TO do
                // 상대방 입력 대기

                ///////////////////////
                ///
                isKeyDown = true;
                ready.myStart = false;
                ready.otherStart = false;

                videoManager.Video.Play();
                IsGamePlaying = true;

                /* =========================================
                 * 템포 조절 인터액션
                 */
                //MidiFilePlayer.MPTK_RePlay();
                NodToTempo.isStart = true;
                ControlTempo.TargetTempo = MusicDataReader.MusicData.Tempo;
                Debug.Log("C: "+ ControlTempo.TargetTempo);
                NodToTempo.GlobalTempo = MusicDataReader.MusicData.Tempo;
                // =========================================

                GuitaristAnimator.SetBool(isGuitarPlayingHash, true);
            }

            // 키보드 입력이 없으면 다음 프레임에 이 위치로 다시 돌아온다.
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        // 비디오가 끝나면 다음 단계로 넘어간다.
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
     * 한 라운드를 끝내는 코루틴
     */
    private IEnumerator RoundEnding()
    {
        Debug.Log("Round Ending...");

        bool isKeyDown = false;

        while (!isKeyDown)
        {
            // ESC를 누르면 게임 완전히 종료
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isKeyDown = true;
                IsGameEnd = true;
            }
            // ESC를 제외한 키를 누르면 다음 라운드 시작
            else if (Input.anyKeyDown)
            {
                isKeyDown = true;
            }

            yield return null;
        }

        Debug.Log("Round Ending Done");
    }
}
