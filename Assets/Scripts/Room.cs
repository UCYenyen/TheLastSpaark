using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public GameObject roomCamera;
    public ParticleSystem roomParticle;
    public Tilemap roomBurningTile;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.currentRoom = null;
            PlayerController.instance.currentRoom = this;
            roomCamera.SetActive(true);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.currentRoom = this;
            roomCamera.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomCamera.SetActive(false);
            PlayerController.instance.currentRoom = null;
        }
    }
}
