using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class LobyNetworkMeneger : MonoBehaviourPunCallbacks
{
    public static LobyNetworkMeneger Instance;
    [SerializeField] TMP_Text waitBattleText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        WindowsManager.Layout.OpenLayout("Loading");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
    }

    public void ToButton()
    {
        WindowsManager.Layout.OpenLayout("AutamaticBattel");
        PhotonNetwork.JoinRandomRoom();

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (returnCode == (short)ErrorCode.NoRandomMatchFound)
        {
            waitBattleText.text = "к≥мнату не знайдену (помилка1), створюЇмо нову";
            CreatNewRoom();     
        }
    }

    void CreatNewRoom()
    {
        RoomOptions currentRoom = new RoomOptions();
        currentRoom.IsOpen = true;
        currentRoom.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RoomNamrGenerator(), currentRoom);
    }

    string RoomNamrGenerator()
    {
        string roomCode = null;
        short codeLenth = 12;

        for(int i = 0; i < codeLenth; i++)
        {
            char symbol = (char)Random.Range(65, 91);
            roomCode += symbol;
        }

        return roomCode;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (returnCode == (short)ErrorCode.GameIdAlreadyExists)
        {
            CreatNewRoom();
        }
    }

    public override void OnCreatedRoom()
    {
        waitBattleText.text = "оч≥куЇмо на другого гравц€...";
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient) return;
        waitBattleText.text = "Ѕитва скоро почнетьс€!";

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Room currentRoom = PhotonNetwork.CurrentRoom;
        currentRoom.IsOpen = false;

        Invoke(nameof(LoadingGameMap), 3f);
        waitBattleText.text = "б≥й через 3 секунди";
        




    }

    void LoadingGameMap()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void StopFindBattelButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
    }
}
