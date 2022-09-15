using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System;
using Valve.VR;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform lefthand;
    public Transform righthand;
    private PhotonView photonView;

    public GameObject componentManager;
    public Components component;

    public SteamVR_Action_Boolean Grab;



    void Start()
    {
        //phtonView를 이용해서 prefab이 spawn by us or another player에 의해 이루어졌는지 파악이 가능하다. 
        photonView = GetComponent<PhotonView>();
        componentManager = GameObject.Find("Component Saver");
        component = componentManager.GetComponent<Components>();

    }

    // Update is called once per frame
    void Update()
    { 



    }

}

