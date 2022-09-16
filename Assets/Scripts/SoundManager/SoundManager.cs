using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject SoundSource;
    public Queue<GameObject> SoundSourcePool = new Queue<GameObject>();
    public AudioSource SnareSound;
    public AudioSource CymbalSound;
    public RPCManager RPCManager;

    [SerializeField]
    private float bassPitch = 25;
    private int soundSourcePoolCount = 30;


    // Start is called before the first frame update
    void Start()
    {
        GenerateNotePool();
    }


    void GenerateNotePool()
    {
        for (int i = 0; i < soundSourcePoolCount; i++)
        {
            SoundSourcePool.Enqueue(Instantiate<GameObject>(SoundSource, gameObject.transform));
        }
    }


    public void Play(float pitch)
    {
        if (SoundSourcePool.Count < 0)
        {
            Debug.LogError("Sound source pool is empty");
            return;
        }

        StartCoroutine(MakeSound(pitch - bassPitch));
    }

    private IEnumerator MakeSound(float pitch)
    {
        GameObject soundSource = SoundSourcePool.Dequeue();
        AudioSource audioSource = soundSource.GetComponent<AudioSource>();
        audioSource.pitch = Mathf.Pow(2f, pitch / 12.0f);
        audioSource.volume = 1.0f;
        audioSource.Play();
        yield return new WaitForSeconds(3.5f);
   
        SoundSourcePool.Enqueue(soundSource);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Play(30);
        }
    }

    public void GenerateDrumSound(string drum)
    {
        if (RPCManager != null) RPCManager.MakeDrumSound(drum);

        switch (drum)
        {
            case "Snare":
                SnareSound.Play();
                break;

            case "Cymbal":
                CymbalSound.Play();
                break;
        }
    }
}
