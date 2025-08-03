using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    [Header("Transition")]
    public CanvasGroup fadePanel;
    bool fadeIn = false;
    bool fadeOut = false;

    [Header("TakeDamageEffect")]
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

    [Header("Quest Menu")]
    public GameObject questPanel;
    [HideInInspector] public bool isQuestMenuOpen = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (SceneManager.GetActiveScene().name != "IntroCutscene")
            {
                DontDestroyOnLoad(this.gameObject);
            }
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
        isQuestMenuOpen = false;
        questPanel.SetActive(false);

        if (isPaused)
        {
            pausePanel.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f; // Resume the game
        }
        else
        {
            pausePanel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f; // Pause the game
        }
    }
    public void QuestMenu()
    {
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
}
