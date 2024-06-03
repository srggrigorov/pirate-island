using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OptionsScreen : MenuOptionScreen
{
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private TMP_Text _musicToggleText;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private TMP_Text _soundToggleText;

    private ISoundService _soundService;

    [Inject]
    public void Construct(ISoundService soundService)
    {
        _soundService = soundService;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _musicToggle.onValueChanged.AddListener(EnableMusic);
        _soundToggle.onValueChanged.AddListener(EnableSound);
    }

    private void EnableMusic(bool value)
    {
        _soundService.EnableMusic(value);
        _musicToggleText.text = value ? "ON" : "OFF";
    }

    private void EnableSound(bool value)
    {
        _soundService.EnableSound(value);
        _soundToggleText.text = value ? "ON" : "OFF";
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _musicToggle.onValueChanged.RemoveListener(EnableMusic);
        _soundToggle.onValueChanged.RemoveListener(EnableSound);
    }
}
