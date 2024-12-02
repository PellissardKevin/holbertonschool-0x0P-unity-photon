using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameDisplay : MonoBehaviourPun, IPunObservable
{
	public Text playerNameText; // L'UI Text au-dessus du joueur
	private string playerName;

	void Start()
	{
		if (photonView.IsMine)
		{
			// Assigner le nom local
			playerName = PhotonNetwork.NickName;
		}
		UpdateNameDisplay();
	}

	void UpdateNameDisplay()
	{
		if (playerNameText != null)
		{
			playerNameText.text = playerName;
		}
	}

	// Implémentation de l'interface IPunObservable
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			// Envoyer le nom de ce joueur aux autres
			stream.SendNext(playerName);
		}
		else
		{
			// Recevoir le nom d'un autre joueur
			playerName = (string)stream.ReceiveNext();
			UpdateNameDisplay();
		}
	}
}
