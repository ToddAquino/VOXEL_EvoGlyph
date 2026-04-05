using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
public class HealthComponent: MonoBehaviour, IDamageable, IShieldable
{
    public UnityEvent OnHit;
    public UnityEvent OnDeath;
    [SerializeField] Healthbar healthbar;
    [SerializeField] private int maxHealth;
    public float damageReductionPercent;
    private int currentHealth;
    public bool IsAlive = true;
    public bool IsImmune;

    [Header("Boss Health Settings")]
    public bool isBossHealth = false;
    public int deathThreshold = 25;

    public void InitializeHealth()
    {
        IsImmune = false;
        IsAlive = true;
        currentHealth = maxHealth;
        healthbar.SetupHealthbar(currentHealth);
        //healthbar.ResetHealthbar();
        ShowHealthBar();
    }
    public void SetMaxHealth(int health)
    {
        maxHealth = health;
    }
    public void ActivateBarrierAbility(float damageReductionRate)
    {
        damageReductionPercent += damageReductionRate;
    }
    public void ActivateImmunity()
    {
        IsImmune = true;
    }
    public void ShowHealthBar()
    {
        healthbar.gameObject.SetActive(true);
    }

    public void HideHealthBar()
    {
        healthbar.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth == 0 || !IsAlive) return;
        if (IsImmune)
        {
            IsImmune = false;
            return;
        }
        int damageTaken = damage;
        if (damageReductionPercent > 0)
        {
            float reducedDamage = damage * (1 - damageReductionPercent);
            damageTaken = Mathf.RoundToInt(reducedDamage);
            
            damageReductionPercent = 0;
             
        }
        currentHealth -= damageTaken;
        UIPopUpGenerator.Instance.CreateDamagePopUP(this.transform.position, this.transform.rotation, damageTaken);

        AudioManager.Instance.PlaySFX("damage");
        healthbar.UpdateHealthbar(currentHealth);
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        if (isBossHealth)
        {
            if (currentHealth <= deathThreshold && IsAlive)
            {
                HandleDeath();
                return;
            }
        }
        else
        {
            if (currentHealth == 0 && IsAlive)
            {
                HandleDeath();
                return;
            }
        }

        OnHit?.Invoke();
    }
    private void HandleDeath()
    {
        HideHealthBar();
        IsAlive = false;
        OnDeath?.Invoke();
    }
    public void Heal(int healthGain)
    {
       if (currentHealth == maxHealth) return;

        currentHealth += healthGain;
        healthbar.UpdateHealthbar(currentHealth);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
