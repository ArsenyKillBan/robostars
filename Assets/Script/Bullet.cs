using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField] BulletInfo info;
    Rigidbody rb;
    PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        info.render = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine) return;
        if (other.gameObject.CompareTag("Player"))
        {
            var targetPv = other.GetComponentInParent<PhotonView>();
            if (targetPv != null && targetPv.Owner != null && pv != null)
            {
                if (targetPv.Owner == pv.Owner) return; 
            }
            other.GetComponentInParent<PlaeyrSetings>().TakeDamage(info.damage);
            PhotonNetwork.Destroy(gameObject);
        }
    }
    public void StartMove(Vector3 dir)
    {
        rb.velocity = dir * info.speed;
    }
}
