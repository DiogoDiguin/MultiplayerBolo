using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 300f;
    Rigidbody2D rigidB2D;
    public float bulletTimeLife = 500f;
    float bulletTimeCount;
    public float bulletDamage = 10;


    void Start()
    {
        rigidB2D = GetComponent<Rigidbody2D>();
        rigidB2D.AddForce(transform.up * bulletSpeed, ForceMode2D.Force);
    }
    void Update()
    {
        Destroy(this.gameObject, bulletTimeLife);
    }
    [PunRPC]
    void BulletDestroy()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy(this.gameObject);
        if (collision.CompareTag("Player") && collision.GetComponent<PlayerController>() && collision.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("Player_ID " + collision.GetComponent<PhotonView>().Owner.ActorNumber + "Player_Name " + collision.GetComponent<PhotonView>().Owner.NickName);

            collision.GetComponent<PlayerController>().TakeDamage(-bulletDamage);
            collision.GetComponent<PlayerController>();
            this.GetComponent<PhotonView>().RPC("BulletDestroy", RpcTarget.AllViaServer);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Colliders")
        {
            //Destroy(this.gameObject);
            this.GetComponent<PhotonView>().RPC("BulletDestroy", RpcTarget.AllViaServer);
        }        
    }


}
