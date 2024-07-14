using Managers;
using System;
using TMPro;
using UnityEngine;

namespace Screens
{
    public class EndScreen : Screen
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _endText;
        [Space]
        [SerializeField] private TextMeshProUGUI _previousScoreText;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;

        private const string END_TEXT = "Игра завершена! Вы продержались всего";

        private float _startTime;

        public static EndScreen Instance;

        public override void Clear()
        {
            _endText.text = $"{END_TEXT} 0";

            _previousScoreText.text = $"{PREVIOUS_SCORE_TEXT} {SaveManager.Instance.GetPreviousScore()}";
            _currentScoreText.text = $"{CURRENT_SCORE_TEXT} 0";
            _bestScoreText.text = $"{BEST_SCORE_TEXT} {SaveManager.Instance.GetBestScore()}";
        }

        private void Awake()
        {
            Instance = this;
        }

        protected override void Start()
        {
            base.Start();

            GameManager.Instance.StartGame += GameManager_StartGame;
            GameManager.Instance.EndGame += GameManager_EndGame;

            GameManager.Instance.UpdateCurrentScore += GameManager_UpdateCurrentScore;
            GameManager.Instance.UpdateBestScore += GameManager_UpdateBestScore;
        }

        private void GameManager_StartGame()
        {
            _startTime = Time.time;
        }

        private void GameManager_EndGame()
        {
            _endText.text = $"{END_TEXT} {TimeSpan.FromSeconds(Time.time - _startTime).ToString("mm':'ss")}";
        }

        private void GameManager_UpdateCurrentScore(int score)
        {
            _currentScoreText.text = $"{CURRENT_SCORE_TEXT} {score}";
        }

        private void GameManager_UpdateBestScore(int score)
        {
            _bestScoreText.text = $"{BEST_SCORE_TEXT} {score}";
        }
    }
}