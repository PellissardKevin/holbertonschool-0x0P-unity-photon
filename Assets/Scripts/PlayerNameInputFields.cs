using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // Ajoutez cette directive pour PUN 2

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    private const string playerNamePrefKey = "PlayerName";

    void Start()
    {
        string defaultName = "";
        InputField _inputField = GetComponent<InputField>();

        if (_inputField != null)
        {
            // Charger le nom par défaut depuis les préférences
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }

        // Assigner le nom par défaut à PhotonNetwork
        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Le nom du joueur ne peut pas être vide.");
            return;
        }

        // Assigner le nom du joueur à PhotonNetwork
        PhotonNetwork.NickName = value;

        // Enregistrer le nom dans les préférences
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
