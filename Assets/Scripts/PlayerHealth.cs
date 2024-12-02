using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviourPun, IPunObservable
{
	public float maxHealth = 100f;
	public float currentHealth;

	public Slider healthBarSlider;  // Utilisation d'un Slider pour la barre de santé
	public Text healthText;

	private void Start()
	{
		currentHealth = maxHealth;
		UpdateHealthUI();
	}

	[PunRPC]
	public void TakeDamage(float damage)
	{
		if (!photonView.IsMine) return;  // Vérifie que c'est bien le joueur local

		currentHealth -= damage;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		UpdateHealthUI();

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void UpdateHealthUI()
	{
		// Met à jour la barre de santé pour le joueur local
		if (healthBarSlider != null)
		{
			healthBarSlider.value = currentHealth / maxHealth;  // Remplace la valeur de la barre de santé
		}

		// Met à jour le texte de la santé
		if (healthText != null)
		{
			healthText.text = $"{currentHealth}/{maxHealth}";
		}
	}

	private void Die()
	{
		Debug.Log($"{photonView.Owner.NickName} is dead!");
		PhotonNetwork.LeaveRoom(); // Déconnecte le joueur
	}

	// Implémentation de l'interface IPunObservable pour synchroniser la santé sur tous les clients
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			// Envoie la santé actuelle du joueur aux autres
			stream.SendNext(currentHealth);
		}
		else
		{
			// Reçoit la santé actuelle d'un autre joueur
			currentHealth = (float)stream.ReceiveNext();
			UpdateHealthUI();  // Met à jour l'UI de santé après réception
		}
	}
}
