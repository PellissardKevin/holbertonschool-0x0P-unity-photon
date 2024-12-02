using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public InputField playerNameInput; // UI InputField to get player name
    public Button joinButton; // UI Button to join the game
    public Canvas lobbyCanvas;

    void Start()
    {
        // Set up the Photon connection
        PhotonNetwork.ConnectUsingSettings();

        // Set button listener to join the game when clicked
        joinButton.onClick.AddListener(JoinGame);
    }

    // Connect to the Photon Cloud server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server");
    }

    // Join a random room or create one if none exists
    void JoinGame()
    {
        string playerName = playerNameInput.text;
        PhotonNetwork.NickName = playerName; // Set player name
        PhotonNetwork.JoinRandomRoom(); // Try joining a random room
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // If no room exists, create a new one
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        lobbyCanvas.gameObject.SetActive(false); // Désactiver le Canvas
        PhotonNetwork.Instantiate("PlayerPrefab", new Vector3(0, 0, 0), Quaternion.identity);
    }
}
