using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviourPun, IPunObservable
{
	public float maxHealth = 100f;
	private float currentHealth;

	public Image healthBarFill;
	public Text healthText;

	// Temps pour la transition de la santé
	public float healthChangeDuration = 0.5f;

	void Start()
	{
		currentHealth = maxHealth;
		UpdateHealthUI();
	}

	[PunRPC]
	public void TakeDamage(float damage)
	{
		if (!photonView.IsMine) return;  // Assurez-vous que cela n'est exécuté que pour le joueur local

		currentHealth -= damage;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

		// Lancez la coroutine pour diminuer la barre de santé progressivement
		StartCoroutine(AnimateHealthBarChange());

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	// Coroutine pour animer la réduction de la barre de santé
	private IEnumerator AnimateHealthBarChange()
	{
		float targetFillAmount = currentHealth / maxHealth;
		float startFillAmount = healthBarFill.fillAmount;
		float elapsedTime = 0f;

		while (elapsedTime < healthChangeDuration)
		{
			elapsedTime += Time.deltaTime;
			healthBarFill.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / healthChangeDuration);
			yield return null;
		}

		// Assurez-vous que la barre atteint exactement la valeur cible à la fin
		healthBarFill.fillAmount = targetFillAmount;
		UpdateHealthUI(); // Mise à jour du texte de santé après la modification de la barre
	}

	private void UpdateHealthUI()
	{
		if (healthText != null)
		{
			healthText.text = $"{currentHealth}/{maxHealth}";  // Mise à jour du texte de santé
		}
	}

	private void Die()
	{
		Debug.Log($"{photonView.Owner.NickName} is dead!");
		PhotonNetwork.LeaveRoom(); // Déconnecte le joueur.
	}

	public void ApplyDamage(float damage)
	{
		// Appelle le RPC pour synchroniser les dégâts sur tous les clients.
		photonView.RPC("TakeDamage", RpcTarget.All, damage);
	}

	// Implémentation de l'interface IPunObservable
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			// Envoie la santé actuelle du joueur aux autres clients
			stream.SendNext(currentHealth);
		}
		else
		{
			// Reçoit la santé actuelle du joueur d'un autre client
			currentHealth = (float)stream.ReceiveNext();
			UpdateHealthUI(); // Met à jour l'UI de santé après réception
		}
	}
}
