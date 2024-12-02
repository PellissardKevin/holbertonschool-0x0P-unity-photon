using Photon.Pun;
using UnityEngine;

public class Missile : MonoBehaviourPun
{
    public float damage = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        // Vérifiez si l'objet touché possède un script PlayerHealth
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // Appel RPC pour appliquer les dégâts à tous les clients
            playerHealth.photonView.RPC("TakeDamage", RpcTarget.All, damage);
        }

        if (PhotonNetwork.IsMasterClient || photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("You are not the owner or MasterClient. Cannot destroy the object.");
        }
    }
}
