using Managers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class AudioUI : MonoBehaviour
    {
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Toggle _musicToggle;

        public Action ChangeSoundState;

        private void OnEnable()
        {
            if (!SaveManager.Instance)
            {
                return;
            }

            _soundToggle.isOn = SaveManager.Instance.GetSoundState();
            _musicToggle.isOn = SaveManager.Instance.GetMusicState();
        }

        private void Start()
        {
            _soundToggle.onValueChanged.AddListener((state) => AudioManager.Instance.ChangeSoundState(state));
            _musicToggle.onValueChanged.AddListener((state) => AudioManager.Instance.ChangeMusicState(state));
        }
    }
}