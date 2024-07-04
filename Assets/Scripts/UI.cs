using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _bonusText;
    [Header("Buttons")]
    [SerializeField] private Button _reloadButton;
    [Space]
    [SerializeField] private Toggle _audioButton;
    [SerializeField] private Toggle _musicButton;

    private float _initialTime;

    public static UI Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _initialTime = Time.time;

        _audioButton.isOn = PlayerPrefs.HasKey("Audio") ? PlayerPrefs.GetInt("Audio") == 1 : true;
        _musicButton.isOn = PlayerPrefs.HasKey("Music") ? PlayerPrefs.GetInt("Music") == 1 : true;

        Player.Instance.Health.ChangeHP += Player_ChangeHP;
        Player.Instance.Health.Die += Player_Die;
    }

    private void Player_ChangeHP(int hp)
    {
        UI.Instance.ChangeHPText($"Энергетических щитов осталось: {hp}");
    }

    private void Player_Die()
    {
        ChangeHPText($"Игра завершена! Вы продержались {TimeSpan.FromSeconds(Time.time - _initialTime).ToString("mm':'ss")}");
        _bonusText.gameObject.SetActive(false);
        _reloadButton.gameObject.SetActive(true);
    }

    public void ChangeHPText(string text)
    {
        ChangeText(_hpText, text);
    }

    public void ChangeBonusText(string text)
    {
        ChangeText(_bonusText, text);
    }

    public void ChangeBonusText(float bonusDelay)
    {
        ChangeBonusText($"До появления бонуса осталось {TimeSpan.FromSeconds(bonusDelay):ss} секунд");
    }

    private void ChangeText(TextMeshProUGUI textHandler, string text)  
    { 
        textHandler.text = text;
    }
}
