using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public Gradient healthGradient;
    public Image healthFill;

    [SerializeField]
    private Component characterController;
    private IHasHealth _healthCharacterController;
    
    private void Start()
    {
        if (characterController is IHasHealth)
        {
          _healthCharacterController = (IHasHealth)characterController;   
        }
        
       SetMaxHealth(_healthCharacterController.HealthSystem.MaxHealth);
    }


    private void SetMaxHealth(int maxHealth)
    {
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;

        healthFill.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int currentHealth)
    {
        healthBarSlider.value = currentHealth;

        healthFill.color = healthGradient.Evaluate(healthBarSlider.normalizedValue);
    }
}
