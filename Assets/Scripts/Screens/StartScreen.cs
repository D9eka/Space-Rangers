using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class StartScreen : Screen
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _previousScoreText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [Header("Buttons")]
        [SerializeField] private Button _startButton;

        public static StartScreen Instance;

        public override void Clear()
        {
            _previousScoreText.text = $"{PREVIOUS_SCORE_TEXT} {SaveManager.Instance.GetPreviousScore()}";
            _bestScoreText.text = $"{BEST_SCORE_TEXT} {SaveManager.Instance.GetBestScore()}";
        }

        private void Awake()
        {
            Instance = this;
        }

        protected override void Start()
        {
            base.Start();

            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(() => GameManager.Instance.OnStartGame());
        }
    }
}