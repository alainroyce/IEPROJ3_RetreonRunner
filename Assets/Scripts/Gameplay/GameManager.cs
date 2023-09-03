using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    #region Fields
    [SerializeField] private bool gameOver;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private bool hasGameStarted;
    [SerializeField] private GameObject startingText;

    [SerializeField] private PlayerMaster player;
    #endregion

    #region Properties
    public bool GameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }

    public bool HasGameStarted
    {
        get { return hasGameStarted; }
        private set { hasGameStarted = value; }
    }
    #endregion


    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        hasGameStarted = false;

        GestureManager.Instance.OnTapEvent += OnTap;
    }

    void OnDisable()
    {
        GestureManager.Instance.OnTapEvent -= OnTap;
    }

    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            hasGameStarted = false;
            gameOverPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !hasGameStarted)
        {
            hasGameStarted = true;
            Destroy(startingText);
        }
    }

    //public void ReplayGame()
    //{
    //    hasGameStarted = true;
    //    player.Reset();
    //    gameOverPanel.SetActive(false);
    //}

    private void OnTap(object send, TapEventArgs args)
    {
        hasGameStarted = true;
        Destroy(startingText);
    }
}
