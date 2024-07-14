using Managers;
using System;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private DifficultySO[] _difficulties;

    private int _currentDifficultyIndex;

    public DifficultySO CurrentDifficulty => _difficulties[_currentDifficultyIndex];

    public Action<DifficultySO> ChangeDifficulty;

    public static DifficultyManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.ClearGame += GameManager_ClearGame;

        GameManager.Instance.UpdateCurrentScore += GameManager_UpdateCurrentScore;
    }

    private void GameManager_ClearGame()
    {
        _currentDifficultyIndex = -1;
        OnChangeDifficulty();
    }

    private void GameManager_UpdateCurrentScore(int score)
    {
        int nextDifficultyIndex = _currentDifficultyIndex + 1;
        if (nextDifficultyIndex < _difficulties.Length && score >= _difficulties[nextDifficultyIndex].ScoreToActivate) 
        { 
            OnChangeDifficulty();
        }
    }

    private void OnChangeDifficulty()
    {
        _currentDifficultyIndex += 1;
        ChangeDifficulty?.Invoke(_difficulties[_currentDifficultyIndex]);
    }    
}