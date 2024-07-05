using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _endScreen;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _endGameText;

    private float _initialTime;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.StartGame += GameManager_StartGame;
        GameManager.Instance.EndGame += GameManager_EndGame;
        GameManager.Instance.ClearGame += GameManager_ClearGame;

        GameManager.Instance.PauseGame += GameManager_PauseGame;
        GameManager.Instance.ResumeGame += GameManager_ResumeGame;
    }

    private void GameManager_StartGame()
    {
        _initialTime = Time.time;

        _startScreen.SetActive(false);
        _endScreen.SetActive(false);
    }

    private void GameManager_EndGame()
    {
        _endScreen.SetActive(true);
        _endGameText.text = $"Игра завершена! Вы продержались всего {TimeSpan.FromSeconds(Time.time - _initialTime).ToString("mm':'ss")}";
    }

    private void GameManager_ClearGame()
    {
        _pauseScreen.SetActive(false);
        _endScreen.SetActive(false);

        _startScreen.SetActive(true);
    }

    private void GameManager_PauseGame()
    {
        _pauseScreen.SetActive(true);
    }

    private void GameManager_ResumeGame()
    {
        _pauseScreen.SetActive(false);
    }
}
