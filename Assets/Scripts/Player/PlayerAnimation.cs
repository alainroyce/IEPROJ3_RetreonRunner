using UnityEngine;


public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        animator.SetBool("isGameStarted", GameManager.Instance.HasGameStarted);
        animator.SetBool("isGrounded", movement.IsGrounded);
        animator.SetBool("isSliding", movement.IsSliding);
    }
}
