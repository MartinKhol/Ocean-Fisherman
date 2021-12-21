using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public PlayerState player;

    //oxygen
    public Button[] OxygenButtons;

    public int oxygenUpgrade = 0;
    public int oxygenPerUpgrade = 30;
    public int oxygenUpgradePrice = 300000;

    //dash
    public Button dashButton;
    public int dashPrice = 500000;

    //health
    public Button[] healthButtons;
    public int healthPrice = 250000;

    //heal
    public int healPrice = 50000;

    //light
    public Button lightButton;
    public GameObject lightMask;
    public int lightPrice = 300000;

    //win
    public int victoryPrice = 1000000;
    public UnityEvent OnVictory;

    public void UpgradeOxygen()
    {
        if (player.Pay(oxygenUpgradePrice))
        {
            OxygenButtons[oxygenUpgrade].interactable = false;
            oxygenUpgrade++;
            player.IncreaseMaxOxygen(oxygenPerUpgrade);
            
        }
    }
    
    public void UnlockDash()
    {
        if (player.Pay(dashPrice))
        {
            player.GetComponent<PlayerMovement>().dashUnlocked = true;
            dashButton.interactable = false;
        }
    }

    private int healthUpgrades = 0;

    public void AddHearts()
    {
        if (player.Pay(healthPrice))
        {
            player.IncreaseMaxHP();
            healthButtons[healthUpgrades].interactable = false;
            healthUpgrades++;
        }
    }

    public void Heal()
    {
        if (player.HasMaxHealth()) return;

        if (player.Pay(healPrice))
        {
            player.Heal();
        }
    }

    public void UnlockLight()
    {
        if (player.Pay(lightPrice))
        {
            lightMask.SetActive(true);
            lightButton.interactable = false;
        }
    }

    public void Win() 
    {
        if (player.Pay(victoryPrice))
        {
            OnVictory.Invoke();
        }
    }
}
