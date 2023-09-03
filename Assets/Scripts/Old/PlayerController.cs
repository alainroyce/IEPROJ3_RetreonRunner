using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;


    private int desiredLane = 1;
    public float laneDistance = 4;

    public float jumpForce;
    public float Gravity = -20;

    public Animator animator;
    private bool isSliding = false;
    // Start is called before the first frame update
    void Start()
    {
        controller= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerManager.isGameStarted)
            return;

        //increase speed 
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }

        animator.SetBool("isGameStarted", true);

        direction.z= forwardSpeed;

       // isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
       // animator.SetBool("isGrounded", isGrounded);
        if (controller.isGrounded)
        {
            //direction.y = 0;
            if (SwipeManager.swipeUp)
            {
                Jump();
                animator.SetBool("isGrounded", false);
            }
            else
            {
                animator.SetBool("isGrounded", true);
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;


        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if(SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }

        if (SwipeManager.swipeRight) 
        {
            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);
            desiredLane++;
            if(desiredLane == 3) 
            {
                desiredLane = 2;
            }
        }
        if (SwipeManager.swipeLeft)
        {
            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);
        //transform.position= targetPosition;
       // controller.center = controller.center;

        if(transform.position == targetPosition)
        {
            return;
        }
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized* 25 * Time.deltaTime;
        if(moveDir.sqrMagnitude > diff.sqrMagnitude) 
        {
            controller.Move(moveDir);
        }
        else
        {
            controller.Move(moveDir);
        }


    }


    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        controller.Move(direction*Time.fixedDeltaTime);

    }
    private void Jump()
    {
        direction.y = jumpForce;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag =="Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(1.3f);
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding= false;   

    }
}
