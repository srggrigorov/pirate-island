using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScreen : MenuOptionScreen
{
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private TMP_Text _musicToggleText;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private TMP_Text _soundToggleText;

    protected override void OnEnable()
    {
        base.OnEnable();
        _musicToggle.onValueChanged.AddListener(EnableMusic);
        _soundToggle.onValueChanged.AddListener(EnableSound);
    }

    private void EnableMusic(bool value)
    {
        SoundManager.Instance.EnableMusic(value);
        _musicToggleText.text = value ? "ON" : "OFF";
    }

    private void EnableSound(bool value)
    {
        SoundManager.Instance.EnableSound(value);
        _soundToggleText.text = value ? "ON" : "OFF";
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _musicToggle.onValueChanged.RemoveListener(EnableMusic);
        _soundToggle.onValueChanged.RemoveListener(EnableSound);
    }
}