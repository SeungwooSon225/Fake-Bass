using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SeungHoon.Utility;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{

    private GameObject spawnedPlayerPrefab;

    //우리가 room을 join 때
    //spawn a Player when joined
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //Make Instantiate gate ->이게 뭐꼬??
        //transform.position , transform.rotation 부분 -> 이부분이 해당 player spawning위치를 지정해준다.
        //여기서는 transform.position , transform.rotation을 통해 gameobject 부분에다가 그냥 spawn시킴
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Player1", transform.position , transform.rotation);
        //spawnedPlayerPrefab.transform.Find("Camera").gameObject.SetActive(true);
        
    }

    //우리가 room을 left 때
    //dismove a Player when left
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        //파괴해주기 -> Destroy all of the client of the server
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
