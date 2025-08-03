using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsourceGiver : MonoBehaviour
{
    public float freezeMeterDecrementSpeed = 1f; // Speed at which the freeze meter increases when near a light source
    private NPC targetNPC;
    void Update()
    {
        if (PlayerController.instance.isNearlightSource)
        {
            PlayerController.instance.DecrementFreezeMeter(freezeMeterDecrementSpeed);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.isNearlightSource = true;
        }
        if (collision.CompareTag("NPC"))
        {
            targetNPC = collision.GetComponent<NPC>();

            if (targetNPC != null)
            {
                targetNPC.isNearLightSource = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.isNearlightSource = false;
        }
        if (collision.CompareTag("NPC"))
        {
            if (targetNPC != null)
            {
                targetNPC.isNearLightSource = false;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.isNearlightSource = true;
        }
        if (collision.CompareTag("NPC"))
        {
            if (targetNPC != null)
            {
                targetNPC.isNearLightSource = true;
                if (targetNPC.currentFreezeMeter > 0 && targetNPC.isFrozen == false)
                {
                    targetNPC.currentFreezeMeter -= freezeMeterDecrementSpeed * Time.deltaTime;
                }
            }
        }
    }
}
