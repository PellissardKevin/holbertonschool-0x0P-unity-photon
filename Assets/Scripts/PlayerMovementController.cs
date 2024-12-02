using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Désactiver le script si ce n'est pas le joueur local
        if (!photonView.IsMine)
        {
            enabled = false;
        }

        // Activer le mode cinématique si nécessaire
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0f, vertical);

        // Animation de marche
        bool isWalking = movement.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine || movement.magnitude <= 0.1f) return;

        // Calcul de la direction et déplacement
        Vector3 moveDirection = movement.normalized * speed * Time.fixedDeltaTime;
        transform.position += moveDirection;

        // Rotation du joueur
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }

    void StickToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f)) // Vérifie si le sol est sous le joueur
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

}
