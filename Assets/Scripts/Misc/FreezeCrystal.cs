using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCrystal : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;

    [Header("Settings")]
    public NPC targetNPC;
    public bool isPlayerFrozenCrystal = false;
    void OnEnable()
    {
        anim.SetBool("isFrozen", true);
        anim.Play("summonCrystal");

        if (!isPlayerFrozenCrystal)
        {
            targetNPC.isFrozen = true;
            targetNPC.freezeAnim.Play("frozen");
            targetNPC.anim.speed = 0f;
        }
        else
        {
            PlayerController.instance.healthController.TakeDamage(1);
            PlayerController.instance.anim.speed = 0f;
        }
    }
    void Update()
    {
        if (isPlayerFrozenCrystal)
        {
            if (PlayerController.instance.isFrozen)
            {
                anim.SetBool("isFrozen", true);
            }
        }
    }
    public void DisableCrystal()
    {
        if (isPlayerFrozenCrystal)
        {
            PlayerController.instance.isFrozen = false;
            PlayerController.instance.currentFreezeMeter = 0f;
            PlayerController.instance.anim.speed = 1f;
        }
        else
        {
            targetNPC.isFrozen = false;
            targetNPC.currentFreezeMeter = 0f;
            targetNPC.freezeAnim.Play("normal");
            PlayerController.instance.isInteracting = false;
            targetNPC.anim.speed = 1f;
        }
       
        gameObject.SetActive(false);
    }
}
