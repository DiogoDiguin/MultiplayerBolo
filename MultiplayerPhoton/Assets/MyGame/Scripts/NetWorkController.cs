using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
//id de conexão = fe5776e6-be29-4ba5-b859-74c09c7ecce8

public class NetWorkController : MonoBehaviourPunCallbacks
{
    [Header("GO")]
    public GameObject loginGO;
    public GameObject partidasGO;
    public GameObject informactionGO;

    [Header("Player")]
    public InputField playerNameInput;
    string playerNameTemp;
    public GameObject myPlayer;

    [Header("Room")]
    public InputField roomName;

    [Header("InforMaction")]
    public Text Info;

    public Text TextInfo;
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.ConnectToRegion("jp");
        playerNameTemp = "Player" + Random.Range(1000, 10000);
        playerNameInput.text = playerNameTemp;

        roomName.text = "Room" + Random.Range(1000, 10000);

        loginGO.gameObject.SetActive(true);
        partidasGO.gameObject.SetActive(false);
        informactionGO.gameObject.SetActive(false);
    }
    public void BtLogin()
    {
        if (playerNameInput.text != "")
        {
            PhotonNetwork.NickName = playerNameInput.text; // Pode ser usado direto sem if
            Debug.Log("Usuario logado com " + PhotonNetwork.NickName);
        }
        else
        {
            PhotonNetwork.NickName = playerNameTemp;
            Debug.Log("Usuario logado com " + PhotonNetwork.NickName);
        }

        PhotonNetwork.ConnectUsingSettings();

        loginGO.gameObject.SetActive(false);
        //partidasGO.gameObject.SetActive(true);
    }

    public void BtBuscarPartidaRapida()
    {
        PhotonNetwork.JoinLobby();
    }
    public void BtCriarSala()
    {
        string roomNameTemp = roomName.text;
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 20 };
        PhotonNetwork.JoinOrCreateRoom(roomNameTemp, roomOptions, TypedLobby.Default);
    }
    //############MonoBehaviourPunCallbacks########
    //Efetua a Conexão
    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }
    //validação
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        // mostra a região em que esta conectando o server
        Debug.Log("My Server: " + PhotonNetwork.CloudRegion + "  /  Ping: " + PhotonNetwork.GetPing());
        partidasGO.gameObject.SetActive(true);
        //Criar um loby
        //PhotonNetwork.JoinLobby();
    }
    //Entra em uma sala aleatoria
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }
    //Casso ocarra uma falha ira criar uma sala randomica
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomTemp = "Room" + Random.Range(1000, 10000) + Random.Range(1000, 10000);
        PhotonNetwork.CreateRoom(roomTemp);
        Debug.Log(roomTemp);
    }
    //Entrou na sala
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log("Romm Name " + PhotonNetwork.CurrentRoom.Name); // numero da sala
        Debug.Log("Current Player in Room " + PhotonNetwork.CurrentRoom.PlayerCount); // quantidade de players  na sala

        Info.text = ("Nome do Player: " + PhotonNetwork.NickName + " Nome da Sala: " + PhotonNetwork.CurrentRoom.Name);
        informactionGO.SetActive(true);
        partidasGO.SetActive(false);

        //Instantiate(myPlayer, myPlayer.transform.position, myPlayer.transform.rotation);

        PhotonNetwork.Instantiate(myPlayer.name, myPlayer.transform.position, myPlayer.transform.rotation, 0);
        InfoPlayer();
    }

    //Motivo da queda de do jogo
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDiconnected: " + cause);
    }
    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    InfoPlayer(roomList);
    //}
    //private void InfoPlayer(List<RoomInfo> roomList)
    private void InfoPlayer()
    {
        //Debug.Log("Name = " + PhotonNetwork.NickName);
        //Debug.Log("Room Name = " + PhotonNetwork.CurrentRoom.Name);
        //Debug.Log("Room Count Players = " + PhotonNetwork.CurrentRoom.PlayerCount);
        //Debug.Log("Room Players = " + PhotonNetwork.CurrentRoom.Players);
        foreach (var item in PhotonNetwork.PlayerList)
        {
            Debug.Log(" NAME = " + item.NickName);
            Debug.Log(" MASTER CLIENT = " + item.IsMasterClient);
            Debug.Log(" PLAYER ACTIVE = " + item.IsInactive);
            Debug.Log(" SERVER ID = " + item.UserId);
            TextInfo.text = item.NickName + item.IsMasterClient;
        }
        //foreach (var item in roomList)
        //{
        //    Debug.Log(" LAYER NAME = " + item.Name);
        //    Debug.Log(" PLAYER OPEM = " + item.IsOpen);
        //    Debug.Log(" PLAYER IS VISIBLE = " + item.IsVisible);
        //    Debug.Log(" MAX PLAYER = " + item.MaxPlayers);
        //    Debug.Log(" MAX COUNT = " + item.PlayerCount);
        //}
    }
}
