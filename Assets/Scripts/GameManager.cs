using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Assignez votre prefab dans l'inspecteur

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Instancier le joueur sur le réseau
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Not connected to Photon Network!");
        }
    }
}
