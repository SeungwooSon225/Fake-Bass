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
    private ScoreSystem allScore;

    public Transform head;
    public Transform lefthand;
    public Transform righthand;

    public Transform hmd;
    public Transform leftController;
    public Transform rightController;

    public GameObject otherHead;
    public GameObject otherLeftHand;
    public GameObject otherRightHand;

    // Start is called before the first frame update
    void Start()
    {
        readyManager = GameObject.Find("isReady");
        ready = readyManager.GetComponent<Components>();
        photonView = GetComponent<PhotonView>();
        VRControllerInputManager = GameObject.Find("VRControllerInputManager").GetComponent<VRControllerInputManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        allScore = GameObject.Find("GlobalScore").GetComponent<ScoreSystem>();

        hmd = GameObject.Find("Camera").GetComponent<Transform>();
        leftController = GameObject.Find("Controller (left)").GetComponent<Transform>();
        rightController = GameObject.Find("Controller (right)").GetComponent<Transform>();

        if (photonView.IsMine)
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.RPCManager = this;

            GameObject hitbar = GameObject.Find("HitBar");
            if (hitbar != null) hitbar.GetComponent<HitBar>().RPCManager = this;

            GameObject[] drumSticks = GameObject.FindGameObjectsWithTag("DrumStick");
            if (drumSticks != null)
            {
                foreach (GameObject drumStick in drumSticks)
                { 
                    drumStick.GetComponent<DrumSoundGenerator>().RPCManager = this;
                }
            }
        }

        if (!photonView.IsMine)
        {
            otherHead = GameObject.Find("OtherHead");
            otherLeftHand = GameObject.Find("OtherLeftHand");
            otherRightHand = GameObject.Find("OtherRightHand");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(hmd, head);
            MapPosition(leftController, lefthand);
            MapPosition(rightController, righthand);
        }
        else
        {
            MapPosition(head, otherHead.transform);
            MapPosition(lefthand, otherLeftHand.transform);
            MapPosition(righthand, otherRightHand.transform);
        }
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

    public void DeductScore(float scr)
    {
        Debug.Log("Score deducted");
        photonView.RPC("MissScore", RpcTarget.All, scr);
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
        Debug.Log("Sound Play!");
        soundManager.Play(pitch);
    }

    [PunRPC]
    public void DrumSound(string drum)
    {
        soundManager.GenerateDrumSound(drum);
    }

    [PunRPC]
    public void MissScore(float scr)
    {
        allScore.score -= scr;
    }

    private void MapPosition(Transform from, Transform to)
    {
        to.transform.position = from.transform.position;
        to.transform.rotation = from.transform.rotation;
    }
}
