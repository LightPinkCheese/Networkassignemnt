using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
using System.Globalization;
using Unity.Netcode.Components;

public class Player : NetworkBehaviour
{

    [SerializeField] private GameObject bulletprefab;

    Rigidbody2D rb;
    public float speed;
    public float turningspeed;
    public NetworkVariable<float> health = new();
    public NetworkVariable<bool> isFireReady = new();
    public NetworkVariable<Vector2> Position = new();

    void Start()
    {
        health.Value = 3;
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (IsOwner)
        {
            

           if (Input.GetKey(KeyCode.W))
            {
                MovemmentRpc(1, true);
            }
            if (Input.GetKey(KeyCode.S))
            {
                MovemmentRpc(-1, true);
            }
            if (Input.GetKey(KeyCode.D))
            {
                MovemmentRpc(-1, false);
            }
            if (Input.GetKey(KeyCode.A))
            {
                MovemmentRpc(1, false);
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {   
                ShootRpc();
            }
        }

        if (health.Value <= 0)
        {
            Debug.Log("test");
            Application.Quit();
        }
    }

    [Rpc(SendTo.Server)]
    void MovemmentRpc(float WorS, bool vertical)
    {
        if (vertical)
        {
            rb.AddRelativeForce(WorS * speed * Time.deltaTime * new Vector2(0, 1));
        }
        else
        {
            transform.Rotate(0, 0, Time.deltaTime * turningspeed * WorS);
        }
    }

    [Rpc(SendTo.Server)]
    void ShootRpc()
    {
            GameObject Bullet = Instantiate(bulletprefab, transform.position, transform.rotation);
            Bullet.GetComponent<NetworkObject>().Spawn();
        
    }
    
    [Rpc(SendTo.Server)]
    public void TakeDamageRpc()
    {
        health.Value -= 1;
    }


}
