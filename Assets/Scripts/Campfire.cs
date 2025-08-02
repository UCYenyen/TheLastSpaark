using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public GameObject popUp;
    bool canRefillTorch = false;
    bool isLit = true;
    void Update()
    {
        if (isLit)
        {
            if (canRefillTorch)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerController.instance.playerTorch.RefillTorch(100f);
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            popUp.SetActive(true);
            canRefillTorch = true;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            popUp.SetActive(true);
            canRefillTorch = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            popUp.SetActive(false);
            canRefillTorch = false;
        }
    }
}
