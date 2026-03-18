using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Events;

public class GameNetworkManager : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public UnityEvent OnGameWin;
    [SerializeField] GameObject allPlayerUI;
    PhotonView pv;

    private void Awake()
    {
        pv = gameObject.GetPhotonView();

    }
    private void Start()
    {
        if (!pv.IsMine)
        {
            allPlayerUI.SetActive(false);
            return;
        }
    }

    public void OutOfBattle()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
