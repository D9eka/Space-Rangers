using Managers;
using UnityEngine;

namespace Screens
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        protected const string PREVIOUS_SCORE_TEXT = "�������:";
        protected const string CURRENT_SCORE_TEXT = "�������:";
        protected const string BEST_SCORE_TEXT = "������:";

        public abstract void Clear();

        public virtual void Activate()
        {
            _content.SetActive(true);
        }
        public virtual void Deactivate()
        {
            _content.SetActive(false);
        }

        protected virtual void Start()
        {
            Clear();

            GameManager.Instance.ClearGame += GameManager_ClearGame;
        }

        private void GameManager_ClearGame()
        {
            Clear();
        }
    }
}