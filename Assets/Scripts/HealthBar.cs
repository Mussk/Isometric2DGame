using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public Gradient healthGradient;
    public Image healthFill;

    [SerializeField]
    private PlayerController playerController;

    private void Start()
    {
       SetMaxHealth(playerController.Health.MaxHealth);
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
