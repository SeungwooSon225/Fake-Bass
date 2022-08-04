using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roopsound : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = 0.0f;
            audioSource.Play();
        }
    }
}
