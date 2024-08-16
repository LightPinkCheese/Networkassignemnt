using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    public float bulletspeed;
    Rigidbody2D rb;
    private float bulletRegistry;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(0, 1) * bulletspeed);
    }

    private void Update()
    {
        bulletRegistry += Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena"))
            DeleteBulletRpc();
        else if (bulletRegistry >= 0.5)
        {
            collision.gameObject.GetComponent<Player>().TakeDamageRpc();
            DeleteBulletRpc();
        }
    }

    [Rpc(SendTo.Server)]
    void DeleteBulletRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }
}
