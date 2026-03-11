using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlaeyrSetings : MonoBehaviourPunCallbacks
{
    [SerializeField] int health = 100;
    int maxHelth;
    [SerializeField] Slider helthBar;
    PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
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
            health = maxHelth;
            GetComponentInChildren<PlayerControler>().Respawn();
        }
        helthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        pv.RPC(nameof(UpdateHealth), RpcTarget.All, damage);
    }
}
