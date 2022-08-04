using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoteInfoQueueManager : MonoBehaviour
{
    public Queue<NoteInfo> NoteInfoQueue = new Queue<NoteInfo>();


    public void MakeNoteQueue(List<NoteInfo> noteInfoList)
    {
        for (int i = 0; i < noteInfoList.Count; i++)
        {
            NoteInfoQueue.Enqueue(noteInfoList[i]);
        }
    }
}
