using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RPCManager : MonoBehaviour
{
    public GameObject readyManager;
    public Components ready;
    public VRControllerInputManager VRControllerInputManager;
    private PhotonView photonView;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        readyManager = GameObject.Find("isReady");
        ready = readyManager.GetComponent<Components>();
        photonView = GetComponent<PhotonView>();
        VRControllerInputManager = GameObject.Find("VRControllerInputManager").GetComponent<VRControllerInputManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        if (photonView.IsMine)
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.RPCManager = this;

            GameObject hitbar = GameObject.Find("HitBar");
            if (hitbar != null) hitbar.GetComponent<HitBar>().RPCManager = this;

            GameObject soundManager = GameObject.Find("SoundManager");
            if (soundManager != null) soundManager.GetComponent<SoundManager>().RPCManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!photonView.IsMine) return;

        //if (VRControllerInputManager.RightPressed())
        //{
        //    Debug.Log("Message");
        //    photonView.RPC("ButtonPressed", RpcTarget.All);
        //}
    }

    public void StartMusic()
    {
        photonView.RPC("ButtonPressed", RpcTarget.All);
    }

    public void MakeSound(float pitch)
    {
        Debug.Log("RPC Make Sound");
        photonView.RPC("Sound", RpcTarget.Others , pitch);
    }


    public void MakeDrumSound(string drum)
    {
        Debug.Log("RPC Make Drum Sound");
        photonView.RPC("DrumSound", RpcTarget.Others, drum);
    }


    [PunRPC]
    public void ButtonPressed()
    {
        if (photonView.IsMine)
        {
            Debug.Log("준비되었습니다.");
            ready.myStart = true;
        }
        else
        {
            Debug.Log("상대방이 준비되었습니다.");
            ready.otherStart = true;
        }
    }

    [PunRPC]
    public void Sound(float pitch)
    {
        Debug.Log("Sound Play!");
        soundManager.Play(pitch);
    }

    [PunRPC]
    public void DrumSound(string drum)
    {
        soundManager.GenerateDrumSound(drum);
    }
}
