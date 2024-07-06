using Creatures;
using System;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private bool _started;
        private bool _paused;

        private int _currentScore;

        public Action StartGame;
        public Action EndGame;
        public Action ClearGame;

        public Action PauseGame;
        public Action ResumeGame;

        public Action<int> UpdatePreviousScore;
        public Action<int> UpdateCurrentScore;
        public Action<int> UpdateBestScore;

        public static GameManager Instance;

        public void OnStartGame()
        {
            ClearGame?.Invoke();
            StartGame?.Invoke();
            _started = true;
        }

        public void OnClearGame()
        {
            _currentScore = 0;
            ClearGame?.Invoke();
        }

        public void OnResumeGame()
        {
            ResumeGame?.Invoke();
            _paused = false;
        }

        public void UpdateScore()
        {
            _currentScore++;
            UpdateCurrentScore?.Invoke(_currentScore);
            if (_currentScore > SaveManager.Instance.GetBestScore()) 
            { 
                UpdateBestScore?.Invoke(_currentScore);
            }
        }

        public void ReloadGame()
        {
            OnEndGame();
            OnClearGame();
            _paused = false;
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InputManager.Instance.Interact += GameInput_Interact;
            InputManager.Instance.Pause += GameInput_Pause;
            InputManager.Instance.Resume += GameInput_Resume;

            Player.Instance.Health.Die += Player_Die;
        }

        private void GameInput_Interact()
        {
            if (!_started)
            {
                OnStartGame();
            }
        }

        private void GameInput_Pause()
        {
            if (!_paused)
            {
                OnPauseGame();
            }
        }

        private void GameInput_Resume()
        {
            if (_started && _paused)
            {
                OnResumeGame();
            }
        }

        private void Player_Die()
        {
            OnEndGame();
        }

        private void OnEndGame()
        {
            UpdatePreviousScore?.Invoke(_currentScore);
            EndGame?.Invoke();
            _started = false;
        }

        private void OnPauseGame()
        {
            PauseGame?.Invoke();
            _paused = true;
        }
    }
}
