using Managers;
using TMPro;
using UnityEngine;


namespace Screens
{
    public class InGameScreen : Screen
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _scoreCounterText;

        private const string SCORE_COUNTER_TEXT = "";

        public static InGameScreen Instance;

        public override void Clear()
        {
            _scoreCounterText.text = $"{SCORE_COUNTER_TEXT} 0";
        }

        private void Awake()
        {
            Instance = this;
        }

        protected override void Start()
        {
            base.Start();

            GameManager.Instance.UpdateCurrentScore += GameManager_UpdateCurrentScore;
        }

        private void GameManager_UpdateCurrentScore(int score)
        {
            _scoreCounterText.text = $"{SCORE_COUNTER_TEXT} {score}";
        }
    }
}