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

            HitBar hitbar = GameObject.Find("HitBar").GetComponent<HitBar>();
            hitbar.RPCManager = this;
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
        photonView.RPC("Sound", RpcTarget.Others, pitch);
    }


    [PunRPC]
    public void ButtonPressed()
    {
        if (photonView.IsMine)
        {
            Debug.Log("�غ�Ǿ����ϴ�.");
            ready.myStart = true;
        }
        else
        {
            Debug.Log("������ �غ�Ǿ����ϴ�.");
            ready.otherStart = true;
        }
    }

    [PunRPC]
    public void Sound(float pitch)
    {
        soundManager.Play(pitch);
    }
}