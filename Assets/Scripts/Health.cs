using System;
using System.Threading.Tasks;
using UnityEngine;

public class Health
{
    public int MaxHealth { get; private set; }
    private int CurrentHealth { get; set; }

    private readonly HealthBar _healthBar;

    private bool _isInvurable;
    public bool IsInvurable { get => _isInvurable;
        set => _isInvurable = value;
    }

    public event Action OnTakingDamage;

    public event Action OnDeath;
  
    public Health(int maxHealth, HealthBar healthBar)
    {
        this.MaxHealth = maxHealth;
        this._healthBar = healthBar;
       
        CurrentHealth = maxHealth;  
    }

    public void TakeDamage(int amount)
    {
        if (_isInvurable) return;

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        Debug.Log("Current health: " + CurrentHealth);
        OnTakingDamage?.Invoke();
        _healthBar?.SetHealth(CurrentHealth);
        if (CurrentHealth != 0) return;
        OnDeath?.Invoke();
        
    }
    public void Heal(int amount)
    {   
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        Debug.Log("Current health: " + CurrentHealth);
        _healthBar?.SetHealth(CurrentHealth);
    }
    

}