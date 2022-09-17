using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Linq;

public class MusicDataReader : MonoBehaviour
{
    public MusicData MusicData;
    public List<NoteInfo> AdjustedNoteInfoArray;


    private string musicDataPath;


    private void Start()
    {
        //ReadMusicData("Cheap Trick - Surrender");
        ReadMusicData("IU-Blueming");
        AdjustNoteInfoArray();
    }


    public bool ReadMusicData(string musicName)
    {
        musicDataPath = Application.dataPath + "/Resources/MusicData/";
        string jsonFilePath = musicDataPath + musicName + ".json";

        if (File.Exists(jsonFilePath))
        {
            try
            {
                string saveText = File.ReadAllText(jsonFilePath);
                MusicData = JsonUtility.FromJson<MusicData>(saveText);

                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Json Load Error : {e.Message}");

                return false;
            }
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// NoteInfoArray를 마디 단위로 압축시키는 함수
    /// </summary>
    private void AdjustNoteInfoArray()
    {
        int chordIndex = 0;

        if (MusicData.BeatArray.Count > MusicData.NoteInfoArray.Count)
        {
            int beatIndex = 0;

            for (int noteInfoIndex = 0; noteInfoIndex < MusicData.NoteInfoArray.Count - 1; noteInfoIndex++)
            {
                while (beatIndex < MusicData.BeatArray.Count && MusicData.BeatArray[beatIndex] < MusicData.NoteInfoArray[noteInfoIndex + 1].offset)
                {
                    NoteInfo newNoteInfo = new NoteInfo();
                    newNoteInfo.pitch = MusicData.NoteInfoArray[noteInfoIndex].pitch;
                    newNoteInfo.offset = MusicData.BeatArray[beatIndex];

                    AdjustedNoteInfoArray.Add(newNoteInfo);

                    beatIndex++;
                }
            }
        }
        else
        {
            for (int beatIndex = 0; beatIndex < MusicData.BeatArray.Count; beatIndex++)
            {
                //Debug.Log("==============beat index: " + beatIndex + "      " + BeatArray[beatIndex]);

                Dictionary<float, int> subNoteInfoArray = new Dictionary<float, int>();

                while (chordIndex < MusicData.NoteInfoArray.Count && MusicData.BeatArray[beatIndex] > MusicData.NoteInfoArray[chordIndex].offset)
                {
                    //Debug.Log("         chord index: " + chordIndex + "      " + NoteInfoArray[chordIndex].offset + "      " + NoteInfoArray[chordIndex].pitch);

                    if (subNoteInfoArray.ContainsKey(MusicData.NoteInfoArray[chordIndex].pitch))
                    {
                        subNoteInfoArray[MusicData.NoteInfoArray[chordIndex].pitch]++;
                    }
                    else
                    {
                        subNoteInfoArray.Add(MusicData.NoteInfoArray[chordIndex].pitch, 1);
                    }

                    chordIndex++;
                }

                var queryAsc = subNoteInfoArray.OrderByDescending(x => x.Value);

                subNoteInfoArray = queryAsc.ToDictionary(x => x.Key, x => x.Value);

                //foreach (KeyValuePair<float, int> items in subNoteInfoArray)
                //{
                //    Debug.Log(items.Key + ":  " + items.Value);
                //}
                //Debug.Log("-----------------");
                //foreach (var dic in queryAsc)
                //{
                //    Debug.Log(dic.Key + ":  " + dic.Value);
                //}

                if (subNoteInfoArray.Count > 0)
                {
                    //Debug.Log(subNoteInfoArray.ElementAt(0));

                    NoteInfo newNoteInfo = new NoteInfo();
                    newNoteInfo.pitch = (int)subNoteInfoArray.ElementAt(0).Key;
                    newNoteInfo.offset = MusicData.BeatArray[beatIndex];

                    AdjustedNoteInfoArray.Add(newNoteInfo);
                }
            }
        }
    }
}
