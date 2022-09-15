using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//MonoBehaviourPunCallbacks을 이용함으로서
//We'll be able to override some of the initial functions that are being called 
//1. When we are connected to server
//2. When somebody join the server
//3. When we join a room and so on 

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //서버를 시작할때 ConeectToServer한다
        ConnectedToServer();
    }

    void ConnectedToServer() 
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        //When we are connected to Master
        //Below function is called
        base.OnConnectedToMaster();
        Debug.Log("Connected To Server");

        //단순히 server에 connect 되었다는 것으로 share data가 가능한것은 아니다
        //같은 room에 있어야지 share data가 가능하다. 
        //이를 위해 PhotonNetwork.JoinOrCreateRoom()함수를 이용한다.
        //PhotonNetwork.JoinOrCreateRoom();의 요소들을 넣기 위해 다음과 같이 roomOptions를 선언해줘야 한다. 
        
        RoomOptions roomOptions = new RoomOptions();
        //최대 인원수
        roomOptions.MaxPlayers =10;
 
        //IsVisible을 true로 선언함으로서 모든 플레이어가 방을 볼 수 있다(?)
        roomOptions.IsVisible = true;

        //IsOpen을 true로 선언함으로서 room에 들어갈 수 있게 해준다.
        roomOptions.IsOpen = true;
        
        //LobbyType도 선언해줘야 한다. 
        PhotonNetwork.JoinOrCreateRoom("Room 1",roomOptions,TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new Player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
