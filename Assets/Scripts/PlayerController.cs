using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;


    private int desiredLane = 1;
    [SerializeField] private float laneDistance = 1.8f;

    public float jumpForce;
    public float Gravity = -20;

    public Animator animator;
    private bool isSliding = false;

    //lerp stuff
    private float elapsedTime = 0;
    private float laneChangeDuration = 0.03f;
    private bool isLaneChanging = false;
    private float startXPos;
    private float endXPos;

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

        if (!animator.GetBool("isGameStarted"))
        {
            animator.SetBool("isGameStarted", true);
        }
        
        if (SwipeManager.swipeRight && !isLaneChanging) 
        {
            Debug.Log("Swipe Right");
            startXPos = transform.position.x;
            endXPos = transform.position.x + laneDistance;
            isLaneChanging = true;
            controller.enabled = false;
        }
        else if (SwipeManager.swipeLeft && !isLaneChanging)
        {
            Debug.Log("Swipe Right");
            startXPos = transform.position.x;
            endXPos = transform.position.x - laneDistance;
            isLaneChanging = true;
            controller.enabled = false;
        }

        if (isLaneChanging && elapsedTime < laneChangeDuration)
        {
            transform.position = new Vector3(Mathf.Lerp(startXPos, endXPos, elapsedTime / laneChangeDuration), transform.position.y, transform.position.z);
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= laneChangeDuration)
            {
                transform.position = new Vector3(endXPos, transform.position.y, transform.position.z);
                isLaneChanging = false;
                elapsedTime = 0;
                controller.enabled = true;
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + forwardSpeed * Time.deltaTime);
        
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
