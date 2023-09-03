using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMaster : MonoBehaviour
{
    [SerializeField] private Transform startingPos;
    [SerializeField] private int currentLane = 2;
    [SerializeField] private uint numLanes = 3;
    [SerializeField] private float laneDistance = 2.5f;
    [SerializeField] private GameObject collidedMusicalNote = null;

    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    // temp keyboard inputs
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (collidedMusicalNote != null)
            {
                MusicalNote script = collidedMusicalNote.GetComponent<MusicalNote>();
                if (script != null)
                {
                    script.OnMusicalNoteHit();
                    collidedMusicalNote = null;
                }
            }
        }

        if (!GameManager.Instance.HasGameStarted || !playerMovement.IsGrounded || playerMovement.IsSliding)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentLane--;

            if (currentLane < 1)
            {
                currentLane = 1;
            }
            else
            {
                playerMovement.OnChangeLane((currentLane - (numLanes / 2 + 1)) * laneDistance);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentLane++;

            if (currentLane > numLanes)
            {
                currentLane = (int)numLanes;
            }
            else
            {
                playerMovement.OnChangeLane((currentLane - (numLanes / 2 + 1)) * laneDistance);
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            playerMovement.OnJump();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            playerMovement.OnSlide();
        }
    }

    public void Reset()
    {
        transform.position = startingPos.position;
    }

    #region Collision
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            GameManager.Instance.GameOver = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MusicalNote" && collidedMusicalNote == null)
            collidedMusicalNote = other.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MusicalNote" && collidedMusicalNote == null)
            collidedMusicalNote = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (collidedMusicalNote == other.gameObject)
            collidedMusicalNote = null;
    }
    #endregion

    #region Inputs
    void Start()
    {
        GestureManager.Instance.OnTapEvent += OnTap;
        GestureManager.Instance.OnSwipeEvent += OnSwipe;
    }

    void OnDisable()
    {
        GestureManager.Instance.OnTapEvent -= OnTap;
        GestureManager.Instance.OnSwipeEvent -= OnSwipe;
    }

    private void OnTap(object send, TapEventArgs args)
    {
        if (collidedMusicalNote != null)
        {
            MusicalNote script = collidedMusicalNote.GetComponent<MusicalNote>();
            if (script != null)
            {
                script.OnMusicalNoteHit();
                collidedMusicalNote = null;
            }
        }
    }

    private void OnSwipe(object send, SwipeEventArgs args)
    {
        if (!GameManager.Instance.HasGameStarted || !playerMovement.IsGrounded || playerMovement.IsSliding)
            return;

        if (args.SwipeDirection == SwipeEventArgs.SwipeDirections.LEFT)
        {
            currentLane--;

            if (currentLane < 1)
            {
                currentLane = 1;
            }
            else
            {
                playerMovement.OnChangeLane(-laneDistance);
            }
        }
        else if (args.SwipeDirection == SwipeEventArgs.SwipeDirections.RIGHT)
        {
            currentLane++;

            if (currentLane > numLanes)
            {
                currentLane = (int)numLanes;
            }
            else
            {
                playerMovement.OnChangeLane(laneDistance);
            }
        }
        else if (args.SwipeDirection == SwipeEventArgs.SwipeDirections.UP)
        {
            playerMovement.OnJump();
        }
        else if (args.SwipeDirection == SwipeEventArgs.SwipeDirections.DOWN)
        {
            playerMovement.OnSlide();
        }
    }
    #endregion
}
