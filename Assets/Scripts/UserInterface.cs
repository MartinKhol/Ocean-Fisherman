using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Singleton tøída, která obsahuje odkazy na UI elementy a obsluhuje jejich funkce
public class UserInterface : MonoBehaviour
{
    private static UserInterface _instance;

    public static UserInterface Instance { get { return _instance; } }

    [SerializeField] private Text debtText;
    [SerializeField] private Image oxygen;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject[] heartsBackground;
    [SerializeField] private Text carriedText;
    [SerializeField] private GameObject escMenu;
    private NumberFormatInfo moneyFormat; 

    //Awake se zavolá po instanciaci tøídy a nastaví odkaz _instance
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    //start se zavolá pøed prvním snímkem
    private void Start()
    {
        moneyFormat = new NumberFormatInfo();
        moneyFormat.CurrencyGroupSeparator = " ";
        moneyFormat.CurrencySymbol = string.Empty;
        moneyFormat.CurrencyDecimalDigits = 0;
    }

    private bool isMenuOpen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7) || (isMenuOpen && Input.GetKeyDown(KeyCode.Joystick1Button1)))
        {
            isMenuOpen = !isMenuOpen;
            if (isMenuOpen) escMenu.GetComponentInChildren<Slider>().Select();
            escMenu.SetActive(isMenuOpen);
        }
    }

    //aktualizuje zobrazovanou hodnotu kyslíku
    public void SetOxygenLevel(float level)
    {
        oxygen.fillAmount = level;
    }

    //aktualizuje zobrazovanou hodnotu penez
    public void DisplayDebt(int current, int max)
    {
        debtText.text = current.ToString("C", moneyFormat);
    }

    //aktualizuje zobrazovana srdicka
    public void DisplayHearts(int amount)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < amount)
                hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);
        }
    }

    //pocet zobrazenych pozadi za srdci
    public void SetMaxHearts(int amount)
    {
        for (int i = 0; i < heartsBackground.Length; i++)
        {
            if (i < amount)
                heartsBackground[i].SetActive(true);
            else
                heartsBackground[i].SetActive(false);
        }
    }

    public void DisplayCarriedValue(int value)
    {
        carriedText.text = value.ToString("C", moneyFormat);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
