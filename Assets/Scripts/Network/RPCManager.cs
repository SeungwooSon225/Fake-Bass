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

    // Start is called before the first frame update
    void Start()
    {
        readyManager = GameObject.Find("isReady");
        ready = readyManager.GetComponent<Components>();
        photonView = GetComponent<PhotonView>();
        VRControllerInputManager = GameObject.Find("VRControllerInputManager").GetComponent<VRControllerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (VRControllerInputManager.RightPressed())
        {
            photonView.RPC("ButtonPressed", RpcTarget.All);
        }
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
}
