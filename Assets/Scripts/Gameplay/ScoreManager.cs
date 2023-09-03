using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    public static ScoreManager Instance { get; private set; }

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

    // Score
    [SerializeField] private uint currentScore = 0;
    [SerializeField] private uint perfecttHitScore = 100;
    [SerializeField] private uint goodHitScore = 50;

    // Fever Mode
    [SerializeField] private uint maxScoreLevel = 5;
    [SerializeField] private uint currentScoreLevel = 0;
    [SerializeField] private uint maxConsecutiveHits = 5;
    [SerializeField] private uint currentConsecutiveHits = 0;
    [SerializeField] private bool isInFeverMode = false;

    // Event
    public event Action<uint> OnUpdateScoreLevelEvent; 


    public uint CurrentScore
    {
        get { return currentScore; }
        private set { currentScore = value; }
    }

    public uint CurrentScoreLevel
    {
        get { return currentScoreLevel; }
        private set { currentScoreLevel = value; }
    }

    public bool IsInFeverMode 
    { 
        get {  return isInFeverMode; } 
        private set {  isInFeverMode = value; } 
    }

    public void OnMusicalNoteHit(ENoteHitStatus status)
    {
        currentConsecutiveHits++;
        if (currentConsecutiveHits == maxConsecutiveHits && currentScoreLevel < maxScoreLevel)
        {
            currentScoreLevel++;
            currentConsecutiveHits = 0;
            isInFeverMode = currentScoreLevel == maxScoreLevel;

            OnUpdateScoreLevelEvent.Invoke(currentScoreLevel);
        }

        if (status == ENoteHitStatus.Perfect)
        {
            currentScore += isInFeverMode ? perfecttHitScore * 2 : perfecttHitScore;
        }
        else if (status == ENoteHitStatus.Good)
        {
            currentScore += isInFeverMode ? goodHitScore * 2 : goodHitScore;
            if (isInFeverMode)
            {
                currentScoreLevel -= 2;
                isInFeverMode = false;

                OnUpdateScoreLevelEvent.Invoke(currentScoreLevel);
            }
        }
    }

    public void OnMiss()
    {
        if (IsInFeverMode)
        {
            currentScoreLevel = 0;
        }
        else
        {
            currentScoreLevel = currentScoreLevel == 0 ? 0 : currentScoreLevel - 1;
        }

        currentConsecutiveHits = 0;
        OnUpdateScoreLevelEvent.Invoke(currentScoreLevel);
    }
}
