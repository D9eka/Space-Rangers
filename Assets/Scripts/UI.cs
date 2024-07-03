using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _bonusText;

    public static UI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeHPText(string text)
    {
        ChangeText(_hpText, text);
    }

    public void ChangeBonusText(string text)
    {
        ChangeText(_bonusText, text);
    }

    private void ChangeText(TextMeshProUGUI textHandler, string text)  
    { 
        textHandler.text = text;
    }
}
