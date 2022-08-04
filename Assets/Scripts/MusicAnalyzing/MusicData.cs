using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicData
{
    public string MusicName;
    public float Tempo;
    public float BeatTrackTempo;
    public List<NoteInfo> NoteInfoArray;
    public List<float> BeatArray;
    public List<float> OnsetArray;


    public MusicData(string musicName, float tempo, float beatTrackTempo, List<NoteInfo> noteInfoArray, List<float> beatArray, List<float> onsetArray)
    {
        MusicName = musicName;
        BeatTrackTempo = beatTrackTempo;
        Tempo = tempo;
        NoteInfoArray = noteInfoArray;
        BeatArray = beatArray;
        OnsetArray = onsetArray;
    }
}
