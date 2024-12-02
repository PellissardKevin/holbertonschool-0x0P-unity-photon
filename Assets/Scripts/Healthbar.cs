using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider healthBar;
	private PlayerHealth playerHealth;

	private void Start()
	{
		playerHealth = GetComponentInParent<PlayerHealth>();  // Trouve le script PlayerHealth dans le parent du joueur
		if (playerHealth != null)
		{
			healthBar.maxValue = playerHealth.maxHealth;
			healthBar.value = playerHealth.maxHealth;  // Initialise la barre de santé
		}
	}

	private void Update()
	{
		if (playerHealth != null)
		{
			// Assurez-vous que la barre de santé suit la santé actuelle
			healthBar.value = playerHealth.currentHealth;
		}
	}
}
