using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("Transition")]
    public CanvasGroup fadePanel;
    bool fadeIn = false;
    bool fadeOut = false;

    [Header("TakeDamageEffect")]
    public GameObject aboutToFreezePanel;
    public CanvasGroup takeDamagePanel;
    public float takeDamageFadeTime = 0.5f;
    private float takeDamageTimer = 0f;

    [Header("UI References")]
    public Image[] hearts;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    [Header("Sliders")]
    public Image torchSlider;
    public Image freezeMeterSlider;
    public Animator torchAnimator;
    public Animator freezeMeterAnimator;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    [HideInInspector] public bool isPaused = false;
    public Image pauseImage;
    public Sprite pauseSprite;
    public Sprite playSprite;

    [Header("Quest Menu")]
    public GameObject questPanel;

    [Header("Mute/Unmute")]
    public Sprite muteSprite;
    public Sprite unmuteSprite;
    public Image muteButtonImage;
    [HideInInspector] public bool isQuestMenuOpen = false;

    [Header("Inventory")]
    public Inventory inventory;
    public TextMeshProUGUI itemNotificationText;

    [Header("DeathScreen")]
    public GameObject deathScreenPanel;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTakeDamageEffect();
        CalculateFadeInFadeOut();
    }
    public void ShowDeathScreen()
    {
        StartCoroutine(fadeInOutDeath());
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("IntroCutscene");
    }
    public void FadeIn()
    {
        fadeIn = true;
    }
    public void FadeOut()
    {
        fadeOut = true;
    }
    public void TakeDamageEffect()
    {
        takeDamagePanel.alpha = 1f;
        takeDamageTimer = takeDamageFadeTime;
    }
    void CalculateTakeDamageEffect()
    {
        if (takeDamageTimer > 0f)
        {
            takeDamageTimer -= Time.deltaTime;
            takeDamagePanel.alpha = Mathf.Lerp(1f, 0f, 1f - (takeDamageTimer / takeDamageFadeTime));
        }
        else
        {
            takeDamagePanel.alpha = 0f;
        }
    }
    public void InstantFadeInFadeOut()
    {
        StartCoroutine(FadeInOut());
    }
    IEnumerator FadeInOut()
    {
        FadeIn();
        yield return new WaitForSeconds(1f);
        FadeOut();
    }
    IEnumerator fadeInOutDeath()
    {
        FadeIn();
        yield return new WaitForSeconds(1f);
        FadeOut();
        yield return new WaitForSeconds(0.1f);
        deathScreenPanel.SetActive(true);
    }
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
    public void CalculateFadeInFadeOut()
    {
        if (fadeIn)
        {
            fadePanel.gameObject.SetActive(true);
            if (fadePanel.alpha < 1f)
            {
                fadePanel.alpha += Time.deltaTime;
                if (fadePanel.alpha >= 1f)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (fadePanel.alpha > 0f)
            {
                fadePanel.alpha -= Time.deltaTime;

                if (fadePanel.alpha == 0f)
                {
                    fadeOut = false;
                    fadePanel.gameObject.SetActive(false);
                }
            }
        }
    }
    public void Pause()
    {
        PlayButtonSFX();
        isQuestMenuOpen = false;
        questPanel.SetActive(false);

        if (isPaused)
        {
            pausePanel.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f; // Resume the game
            pauseImage.sprite = pauseSprite;
        }
        else
        {
            pausePanel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f; // Pause the game
            pauseImage.sprite = playSprite;
        }
    }
    public void QuestMenu()
    {
        PlayButtonSFX();
        isPaused = false;
        pausePanel.SetActive(false);

        if (isQuestMenuOpen)
        {
            questPanel.SetActive(false);
            isQuestMenuOpen = false;
            Time.timeScale = 1f; // Pause the game
        }
        else
        {
            questPanel.SetActive(true);
            isQuestMenuOpen = true;
            Time.timeScale = 0f; // Resume the game
        }
    }
    public void MuteUnMute()
    {
        PlayButtonSFX();
        GameManager.instance.isMuted = !GameManager.instance.isMuted;

        if (GameManager.instance.isMuted)
        {
            muteButtonImage.sprite = unmuteSprite;
        }
        else
        {
            muteButtonImage.sprite = muteSprite;
        }
    }
    void PlayButtonSFX()
    {
        PlayerController.instance.playerAudio.PlaySFX(0);
    }
}
