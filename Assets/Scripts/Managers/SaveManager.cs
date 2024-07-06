using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private const string PREVIOUS_SCORE = "PreviousScore";
        private const string BEST_SCORE = "BestScore";

        private const string SOUND_STATE = "Sound";
        private const string MUSIC_STATE = "Music";

        public static SaveManager Instance;

        public int GetPreviousScore()
        {
            return GetPlayerPrefInt(PREVIOUS_SCORE);
        }

        public int GetBestScore()
        {
            return GetPlayerPrefInt(BEST_SCORE);
        }

        public bool GetSoundState()
        {
            return GetPlayerPrefBool(SOUND_STATE);
        }

        public bool GetMusicState()
        {
            return GetPlayerPrefBool(MUSIC_STATE);
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.UpdatePreviousScore += GameManager_UpdatePreviousScore;
            GameManager.Instance.UpdateBestScore += GameManager_UpdateBestScore;

            AudioManager.Instance.ChangeSoundState += AudioManager_ChangeSoundState;
            AudioManager.Instance.ChangeMusicState += AudioManager_ChangeMusicState;
        }

        private void GameManager_UpdatePreviousScore(int score)
        {
            SetPlayerPrefInt(PREVIOUS_SCORE, score);
        }

        private void GameManager_UpdateBestScore(int score)
        {
            SetPlayerPrefInt(BEST_SCORE, score);
        }

        private void AudioManager_ChangeSoundState(bool state)
        {
            SetPlayerPrefBool(SOUND_STATE, state);
        }

        private void AudioManager_ChangeMusicState(bool state)
        {
            SetPlayerPrefBool(MUSIC_STATE, state);
        }

        private int GetPlayerPrefInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : defaultValue;
        }

        private bool GetPlayerPrefBool(string key)
        {
            return !PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) == 1;
        }

        private void SetPlayerPrefInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        private void SetPlayerPrefBool(string key, bool value) 
        { 
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }
}