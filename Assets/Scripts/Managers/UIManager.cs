using Screens;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartScreen.Instance.Activate();

            GameManager.Instance.StartGame += GameManager_StartGame;
            GameManager.Instance.EndGame += GameManager_EndGame;
            GameManager.Instance.ClearGame += GameManager_ClearGame;

            GameManager.Instance.PauseGame += GameManager_PauseGame;
            GameManager.Instance.ResumeGame += GameManager_ResumeGame;
        }

        private void GameManager_StartGame()
        {
            StartScreen.Instance.Deactivate();
            EndScreen.Instance.Deactivate();

            InGameScreen.Instance.Activate();
        }

        private void GameManager_EndGame()
        {
            InGameScreen.Instance.Deactivate();

            EndScreen.Instance.Activate();
        }

        private void GameManager_ClearGame()
        {
            InGameScreen.Instance.Deactivate();
            PauseScreen.Instance.Deactivate();
            EndScreen.Instance.Deactivate();

            StartScreen.Instance.Activate();
        }

        private void GameManager_PauseGame()
        {
            InGameScreen.Instance.Deactivate();

            PauseScreen.Instance.Activate();
        }

        private void GameManager_ResumeGame()
        {
            PauseScreen.Instance.Deactivate();

            InGameScreen.Instance.Activate();
        }
    }
}
