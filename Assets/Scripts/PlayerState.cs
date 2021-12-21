using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//General class which contains all player stats

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }

    [SerializeField] private int healthPoints = 3;
    [SerializeField] private float oxygenLeft;
    [SerializeField] private int money = 100000;
    [Space] [SerializeField] private int maxHealthPoints = 3;
    [SerializeField] private int maxOxygen = 20;
    [SerializeField] private int targetDebt = 1000000;
    [SerializeField] private int debtPerSecond = 1000;
    [SerializeField] private int currentlyHolding = 0;

    public UnityEvent OnDeath;
    public UnityEvent OnVictory;
    public UnityEvent OnDamaged;

    private float submergedTime;
    private Animator animator;
    private UserInterface userInterface;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        oxygenLeft = maxOxygen;
        healthPoints = maxHealthPoints;
        userInterface = UserInterface.Instance;
        userInterface.DisplayDebt(money, targetDebt);
        userInterface.DisplayHearts(healthPoints);
        userInterface.SetMaxHearts(maxHealthPoints);
        userInterface.DisplayCarriedValue(currentlyHolding);

        animator = GetComponent<Animator>();
    }

    public void Submerge()
    {
        submergedTime = Time.time;
        lastSecond = Mathf.CeilToInt(Time.time);
    }

    public void Surface()
    {
        //regen oxygen
        oxygenLeft = maxOxygen;
        oxygenWarningPlayed = false;
        userInterface.SetOxygenLevel(1f);

        //heal to full
        //healthPoints = maxHealthPoints;
        //UserInterface.Instance.DisplayHearts(healthPoints);

        //sell items
        if (currentlyHolding > 0)
        {
            SoundManger.Instance.PlayCashIn();
            GetComponent<PlayerCollecter>().DebtReducedText(currentlyHolding);
            money += currentlyHolding;
            currentlyHolding = 0;
            userInterface.DisplayDebt(money, targetDebt);
            userInterface.DisplayCarriedValue(currentlyHolding);

            if (money >= targetDebt)
            {
                OnVictory.Invoke();
            }
        }
    }

    public void AddLoot(int value)
    {
        currentlyHolding += value;
        userInterface.DisplayCarriedValue(currentlyHolding);
    }

    public bool Pay(int value)
    {
        if (money > value)
        {
            money -= value;
            userInterface.DisplayDebt(money, targetDebt);
            return true;
        }
        else
            return false;
    }

    private bool immune;

    public void TakeDamage(int value = 1)
    {
        if (immune) return;
        immune = true;
        healthPoints -= value;
        userInterface.DisplayHearts(healthPoints);
        animator.SetTrigger("Damaged");
        OnDamaged.Invoke();
        if (healthPoints < 1) OnDeath.Invoke();

        Invoke("EndImmunity", 1f);
    }

    private void EndImmunity()
    {
        immune = false;
    }

    private int lastSecond = 0;
    private bool oxygenWarningPlayed = false;

    private void Update()
    {
        if (!PlayerMovement.surfaced)
        {
            //counting oxygen
            oxygenLeft = maxOxygen - (Time.time - submergedTime);
            userInterface.SetOxygenLevel((float)oxygenLeft / (float)maxOxygen);
            if (!oxygenWarningPlayed && (oxygenLeft < (maxOxygen * (1f / 3f))))
            {
                Message.Instance.ShowMessage("Oxygen levels are decreasing!");
                SoundManger.Instance.PlayOxygenWarning();
                oxygenWarningPlayed = true;
            }

            if (oxygenLeft < 0f)
            {
                if (oxygenLeft < -2f)
                {
                    submergedTime += 2;
                    TakeDamage();
                }
            }


            /*
            //counting debt
            if (lastSecond < Mathf.FloorToInt(Time.time))
            {
                money -= debtPerSecond;
                lastSecond = Mathf.FloorToInt(Time.time);
                UserInterface.Instance.DisplayDebt(money, targetDebt);
                if (money == 10000)
                {
                    Message.Instance.ShowMessage("Money is really running out!");
                    SoundManger.Instance.PlayOxygenWarning();
                }
                if (money == 30000)
                {
                    Message.Instance.ShowMessage("Money is running out!");
                    SoundManger.Instance.PlayOxygenWarning();
                }
                
                if (money < 1) OnDeath.Invoke();
            
            }*/
        }
    }

    public void IncreaseMaxOxygen(int addAmount)
    {
        maxOxygen += addAmount;
        oxygenLeft = maxOxygen;
    }

    public void IncreaseMaxHP()
    {
        maxHealthPoints++;
        healthPoints = maxHealthPoints;
        userInterface.DisplayHearts(healthPoints);
        userInterface.SetMaxHearts(maxHealthPoints);
    }

    public bool Heal(int amount = 1)
    {
        if (healthPoints < maxHealthPoints)
        {
            healthPoints++;
            userInterface.DisplayHearts(healthPoints);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasMaxHealth()
    {
        return healthPoints >= maxHealthPoints;
    }
}
