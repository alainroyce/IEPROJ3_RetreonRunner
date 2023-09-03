using System.Collections;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 10;
    [SerializeField] private float jumpHeight = 1;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isSliding = false;

    private const float gravity = -9.81f;
    private float targetX = 0f;
    private float groundY = 0f;

    private CharacterController controller;


    public bool IsGrounded
    {
        get { return controller.isGrounded; }
    }

    public bool IsSliding
    {
        get { return isSliding; }
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        isJumping = false; 
        isSliding = false;
        targetX = transform.position.x;
        groundY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.HasGameStarted)
            return;

        Vector3 currentVelocity = Vector3.zero;

        // calculate x velocity
        if (Mathf.Abs(transform.position.x - targetX) < 0.08f)
        {
            currentVelocity.x += targetX - transform.position.x; // just exactly move to spot
        }
        else
        {
            currentVelocity.x += (targetX - transform.position.x) * 0.3f; // ease in
        }

        // calculate y velocity
        if (isJumping)
        {
            currentVelocity.y += ((groundY + jumpHeight) - transform.position.y) / (groundY + jumpHeight); // ease in
        }
        else
        {
            if (controller.isGrounded && groundY != transform.position.y)
            {
                groundY = transform.position.y;
            }

            float multiplier = 1.3f - (Mathf.Abs(groundY - transform.position.y) / jumpHeight);
            currentVelocity.y += gravity * multiplier * Time.fixedDeltaTime;
        }

        // other calculations
        currentVelocity.z += forwardSpeed * Time.fixedDeltaTime;
        controller.Move(currentVelocity);
    }

    private void LateUpdate()
    {
        if (Mathf.Abs(transform.position.y - (groundY + jumpHeight)) < 0.08f)
        {
            isJumping = false;
        }
    }

    public void OnChangeLane(float target)
    {
        if (!controller.isGrounded)
            return;

        targetX = target;
    }

    public void OnJump()
    {
        if (!controller.isGrounded)
            return;

        isJumping = true;
    }

    public void OnSlide()
    {
        StartCoroutine(SlideCoroutine());
    }

    private IEnumerator SlideCoroutine()
    {
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        isSliding = true;

        yield return new WaitForSeconds(0.75f);
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        isSliding = false;
    }
}
