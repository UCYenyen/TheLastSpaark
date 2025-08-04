using UnityEngine;

public class FreezeCrystal : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;

    [Header("Settings")]
    public NPC targetNPC;
    public bool isPlayerFrozenCrystal = false;
    public AudioManager audioManager;

    [Header("Unfreeze Meter")]
    public GameObject pressE;
    public float unfreezeAmountPerClick;
    public float currentUnfreezeMeter = 0f;
    public float maxUnfreezeMeter = 100f;
    void OnEnable()
    {
        audioManager.PlaySFX(Random.Range(49,53));
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
            pressE.SetActive(true);
            if (PlayerController.instance.isFrozen)
            {
                anim.SetBool("isFrozen", true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentUnfreezeMeter >= maxUnfreezeMeter)
                {
                    DisableCrystal();
                }
                else
                {
                    audioManager.PlaySFX(Random.Range(57, 60));
                    currentUnfreezeMeter += unfreezeAmountPerClick;
                }
            }
        }
    }
    public void DisableCrystal()
    {
        currentUnfreezeMeter = 0f;
        audioManager.PlaySFX(Random.Range(15,21));

        if (isPlayerFrozenCrystal)
        {
            PlayerController.instance.isFrozen = false;
            PlayerController.instance.currentFreezeMeter = 0f;
            PlayerController.instance.anim.speed = 1f;
            pressE.SetActive(false);
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
