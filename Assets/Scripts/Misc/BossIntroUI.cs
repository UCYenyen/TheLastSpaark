using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroUI : MonoBehaviour
{
    public AudioManager audioManager;
    public float timeUntilIntroIsFinished = 3;
    bool introIsFinished;
    public Animator introAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilIntroIsFinished > 0)
        {
            timeUntilIntroIsFinished -= Time.deltaTime;
            if (timeUntilIntroIsFinished <= 0 && !introIsFinished)
            {
                introAnimator.SetTrigger("IntroDone");
            }
        }
    }
    public void PlayBossIntroSFX()
    {
        audioManager.PlaySFX(60);
    }
    public void ChangIntoBossMusic()
    {
        PlayerController.instance.playerAudio.PlayMusic(3);
        MakePlayerIsInteractingTrue();
    }
    public void MakePlayerIsInteractingTrue()
    {
        PlayerController.instance.isInteracting = true;
        BossController.instance.canMove = false;
    }
    public void MakePlayerIsInteractingFalse()
    {
        PlayerController.instance.isInteracting = false;
        BossController.instance.canMove = true;
    }
}
