using Photon.Pun;
using UnityEngine;

public class PlayerAttack : MonoBehaviourPun
{
	public GameObject missilePrefab;
	public Transform launchPoint;
	public float missileSpeed = 10f;

	private Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger("Attack");
		}
	}

	public void LaunchMissile()
	{
		if (missilePrefab == null)
		{
			Debug.LogError("MissilePrefab is not assigned!");
			return;
		}

		if (launchPoint == null)
		{
			Debug.LogError("LaunchPoint is not assigned!");
			return;
		}

		// Instancier un missile sur le réseau
		GameObject missile = PhotonNetwork.Instantiate(missilePrefab.name, launchPoint.position, launchPoint.rotation);

		if (missile == null)
		{
			Debug.LogError("Missile instantiation failed!");
			return;
		}

		Rigidbody rb = missile.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.velocity = launchPoint.forward * missileSpeed;
		}
		else
		{
			Debug.LogError("Rigidbody not found on missilePrefab!");
		}
	}

}
