using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumSoundGenerator : MonoBehaviour
{
    public SoundManager SoundManager;
    public RPCManager RPCManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(RPCManager != null) RPCManager.MakeDrumSound(other.tag);
        SoundManager.GenerateDrumSound(other.tag);
    }
}
