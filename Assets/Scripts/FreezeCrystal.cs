using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCrystal : MonoBehaviour
{
    public Animator anim;
    void OnEnable()
    {
        anim.SetBool("isFrozen", true);
        anim.Play("summonCrystal");
    }
    void Update()
    {
        if (PlayerController.instance.isFrozen)
        {
            anim.SetBool("isFrozen", true);
        }
    }
    public void DisableCrystal()
    {
        PlayerController.instance.isFrozen = false;
        PlayerController.instance.currentFreezeMeter = 0f;
        gameObject.SetActive(false);
    }
}
