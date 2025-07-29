using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class HealthBarUI : MonoBehaviour
{
    public float Health, MaxHealth;
    public float Width = 200f, Height = 20f;

    [SerializeField] private RectTransform healthBar;
    [SerializeField] private TextMeshProUGUI healthText; 

    public void SetMaxHealth(float maxHealth)
    {
        this.MaxHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        this.Health = health;

        if (MaxHealth <= 0 || Width <= 0) return;

        float newWidth = (this.Health / MaxHealth) * Width;
        healthBar.sizeDelta = new Vector3(newWidth, this.Height);

        UpdateHealthText(); 
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{(Health / MaxHealth) * 100:F0}%"; 
        }
    }
}
