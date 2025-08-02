using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [Header("UI References")]
    public Image[] hearts;
    [HideInInspector] public int currentHealth;
    public Slider torchSlider;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
