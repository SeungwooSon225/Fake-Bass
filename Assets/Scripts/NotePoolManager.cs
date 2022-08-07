using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePoolManager : MonoBehaviour
{
    public MusicDataReader MusicDataReader;
    public List<GameObject> NotePool = new List<GameObject>();
    public GameObject Note;
    //public NoteInfoQueueManager NoteInfoQueueManager;
    public Queue<NoteInfo> NoteInfoQueue = new Queue<NoteInfo>();
    public NoteManager NoteManager;
    //public HitBar HitBar;

    private int notePoolCount = 30;
    private float previousPitch;
    private Color currentColor = Color.red;


    // Start is called before the first frame update
    void Start()
    {
        GenerateNotePool();
    }


    void GenerateNotePool()
    {
        for (int i = 0; i < notePoolCount; i++)
        {
            NotePool.Add(Instantiate<GameObject>(Note, gameObject.transform));
        }
    }


    public void SetNoteOnProperPosition(GameObject Note)
    {
        if (NoteInfoQueue.Count > 0)
        {
            NoteInfo noteInfo = NoteInfoQueue.Dequeue();
            Note.transform.localPosition = new Vector3(0, 0, -(noteInfo.offset / MusicDataReader.MusicData.Tempo * 60f) * NoteManager.Scale);
            Note.GetComponent<Note>().pitch = noteInfo.pitch;


            // Change the color of the Note, if the pitch of the Note is different from the previous pitch
            if (previousPitch != noteInfo.pitch)
            {
                ChangeNoteColor();
            }

            //Note.GetComponent<Renderer>().material.color = currentColor;
            Note.GetComponent<Renderer>().material.SetColor("_EmissionColor", currentColor);

            previousPitch = noteInfo.pitch;
        }
        else
        {
            Note.gameObject.SetActive(false);
        }
    }


    public void InitializeNotes()
    {
        previousPitch = NoteInfoQueue.Peek().pitch;

        for (int i = 0; i < NotePool.Count; i++)
        {
            SetNoteOnProperPosition(NotePool[i]);
        }
    }

    void ChangeNoteColor()
    {
        if (currentColor == Color.blue)
        {
            currentColor = Color.red;
        }
        else 
        {
            currentColor = Color.blue;
        }
    
    }

    public void MakeNoteQueue(List<NoteInfo> noteInfoList)
    {
        for (int i = 0; i < noteInfoList.Count; i++)
        {
            NoteInfoQueue.Enqueue(noteInfoList[i]);
        }
    }
}
