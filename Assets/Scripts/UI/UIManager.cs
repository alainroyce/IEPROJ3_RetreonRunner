using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { get; private set; }

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

    [SerializeField] private GameObject hudPanel = null;
    [SerializeField] private TMP_Text scoreText = null;

    [SerializeField] private List<Image> starsList = new();
    [SerializeField] private Color inactiveStarColor;
    [SerializeField] private Color activeStarColor;

    private void Start()
    {
        ScoreManager.Instance.OnUpdateScoreLevelEvent += OnUpdateScoreLevel;
    }

    private void OnDisable()
    {
        ScoreManager.Instance.OnUpdateScoreLevelEvent -= OnUpdateScoreLevel;
    }

    private void Update()
    {
        scoreText.text = "Score: " + ScoreManager.Instance.CurrentScore.ToString();
    }

    private void OnUpdateScoreLevel(uint currentLevel)
    {
        for (int i = 0; i < starsList.Count; i++)
        {
            if (i + 1 <= currentLevel)
            {
                starsList[i].color = activeStarColor;
            }
            else
            {
                starsList[i].color = inactiveStarColor;
            }
        }
    }
}
