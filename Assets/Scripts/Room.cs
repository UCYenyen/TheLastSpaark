using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public PolygonCollider2D roomConfiner;
    public ParticleSystem roomParticle;
    public Tilemap roomBurningTile;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.currentRoom = this;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.instance.currentRoom != this)
            {
                PlayerController.instance.currentRoom = this;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.currentRoom = null;
        }
    }
}
