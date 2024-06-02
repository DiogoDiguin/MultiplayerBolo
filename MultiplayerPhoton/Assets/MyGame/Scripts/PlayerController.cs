using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun
{    
    public float playerSpeed = 5f;
    Rigidbody2D rigidB2D;
    PhotonView photonVieww;

    [Header("Health")]
    public float playerHealthMax = 100f;
    float playerHealtCurrent;
    public Image playerHealthFill;

    [Header(" Bullet ")]
    public GameObject bulletGo;
    public GameObject bulletGoPhotonView;
    public GameObject spawBullet;

    [Header("Machine Gun")]
    public KeyCode machineGunKey = KeyCode.Space;
    public float fireRate = 0.1f; // Segundos entre disparos
    public float machineGunDuration = 2f; // Duração da metralhadora em segundos
    public float machineGunCooldown = 5f; // Tempo de recarga em segundos
    private float nextFireTime = 0f;
    private float machineGunEndTime = 0f;
    private bool isMachineGunActive = false;
    private bool isMachineGunCooldown = false;

    void Start()
    {
        rigidB2D = GetComponent<Rigidbody2D>();
        photonVieww = GetComponent<PhotonView>();
        HealthManager(playerHealthMax);
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            PlayerMove();
            PlayerTurn();
            Shooting();
            UpdateMachineGunState();
        }
    }

    void Shooting()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PhotonNetwork.Instantiate(bulletGoPhotonView.name, spawBullet.transform.position, spawBullet.transform.rotation, 0);
        }
        else if(Input.GetKey(machineGunKey) && !isMachineGunCooldown && !isMachineGunActive)
        {
            isMachineGunActive = true;
            machineGunEndTime = Time.time + machineGunDuration;
        }

        if(isMachineGunActive && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            PhotonNetwork.Instantiate(bulletGoPhotonView.name, spawBullet.transform.position, spawBullet.transform.rotation, 0);
        }
    }

    void UpdateMachineGunState()
    {
        if (isMachineGunActive && Time.time >= machineGunEndTime)
        {
            isMachineGunActive = false;
            isMachineGunCooldown = true;
            Invoke("ResetMachineGunCooldown", machineGunCooldown);
        }
    }

    void ResetMachineGunCooldown()
    {
        isMachineGunCooldown = false;
    }

    [PunRPC]
    void Shoot()
    {
        Instantiate(bulletGoPhotonView, spawBullet.transform.position, spawBullet.transform.rotation);
    }

    public void TakeDamage(float value)
    {
        photonView.RPC("TakeDamageNetWork", RpcTarget.AllBuffered, value);
    }

    [PunRPC]
    void TakeDamageNetWork(float value)
    {
        Debug.Log("TakeManager");
        HealthManager(value);

        if(playerHealtCurrent <= 0)
        {
            Debug.Log("***GAME OVER***");
            Destroy(this.gameObject);
        }
    }

    void HealthManager(float value)
    {
        playerHealtCurrent += value;
        playerHealthFill.fillAmount = playerHealtCurrent / 100f;
        Debug.Log("perdendo vida");
    }

    void PlayerMove()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        rigidB2D.velocity = new Vector2(x, y);
    }

    void PlayerTurn()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    // Realocar Player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Barriers"))
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }
}
