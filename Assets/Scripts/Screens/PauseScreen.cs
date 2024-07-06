using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class PauseScreen : Screen
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _previousScoreText;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [Header("Buttons")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _exitButton;

        public static PauseScreen Instance;

        public override void Clear()
        {
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

            _resumeButton.onClick.RemoveAllListeners();
            _resumeButton.onClick.AddListener(() => GameManager.Instance.OnResumeGame());

            _exitButton.onClick.RemoveAllListeners();
            _exitButton.onClick.AddListener(() => GameManager.Instance.ReloadGame());

            GameManager.Instance.UpdateCurrentScore += GameManager_UpdateCurrentScore;
        }

        private void GameManager_UpdateCurrentScore(int score)
        {
            _currentScoreText.text = $"{CURRENT_SCORE_TEXT} {score}";
        }
    }
}