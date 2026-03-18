using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;


public class PlaeyrSetings : MonoBehaviourPunCallbacks
{
    [SerializeField] int health = 100;
    int maxHelth;
    [SerializeField] Slider helthBar;
    PhotonView pv;
    [SerializeField] TMP_Text resulText;

    GameNetworkManager gameManeger;
    const byte GAME_IS_WIN = 0;
    private void Awake()
    {
        pv = GetComponentInParent<PhotonView>();
        gameManeger = GetComponentInParent<GameNetworkManager>();
    }

    void OnHetworkEventCome(EventData obj)
    {
        if (obj.Code == GAME_IS_WIN)
        {
            if (!pv.IsMine) return;
            gameManeger.OnGameWin.Invoke();
       
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnHetworkEventCome;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnHetworkEventCome;
    }
    void SendWinEvent()
    {
        Object[] datas = null;
        PhotonNetwork.RaiseEvent(GAME_IS_WIN, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
    private void Start()
    {
        maxHelth = health;
        helthBar.maxValue = maxHelth;
        helthBar.value = health;
    }
    [PunRPC]
    public void UpdateHealth(int value)
    {
        health -= value;
        if(health <= 0)
        {
            if (!pv.IsMine) return;
            SendWinEvent();
            gameManeger.OnGameOver.Invoke();
            resulText.text = "You Looser!!";
        }
        helthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        pv.RPC(nameof(UpdateHealth), RpcTarget.All, damage);
    }
}
