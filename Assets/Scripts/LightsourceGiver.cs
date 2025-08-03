using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsourceGiver : MonoBehaviour
{
    public float freezeMeterIncrementSpeed = 1f; // Speed at which the freeze meter increases when near a light source
    void Update()
    {
        if (PlayerController.instance.isNearlightSource)
        {
            PlayerController.instance.DecrementFreezeMeter(freezeMeterIncrementSpeed);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.isNearlightSource = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.isNearlightSource = false;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.isNearlightSource = true;
        }
    }
}
