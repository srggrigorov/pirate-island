using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")] [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private List<MenuOptionButton> _menuOptionButtons;

    [Space(5)] [SerializeField] private GameObject _menuCamera;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(HandlePlayButtonClick);
        _quitButton.onClick.AddListener(Application.Quit);
        foreach (var optionButton in _menuOptionButtons)
        {
            optionButton.Button.onClick.AddListener(() => OpenMenuOptionScreen(optionButton.MenuOptionScreen));
        }
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(HandlePlayButtonClick);
        _quitButton.onClick.RemoveListener(Application.Quit);
        foreach (var optionButton in _menuOptionButtons)
        {
            optionButton.Button.onClick.RemoveAllListeners();
        }
    }
    
    //Made by Sergei Grigorov

    private void HandlePlayButtonClick()
    {
        _menuCamera.SetActive(false);
        StartGame();
        gameObject.SetActive(false);
    }

    private async void StartGame()
    {
        await Task.Delay(1000);
        _gameManager.StartGame();
    }

    private void OpenMenuOptionScreen(MenuOptionScreen screen)
    {
        screen.MainMenu ??= this;
        screen.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}

[Serializable]
internal class MenuOptionButton
{
    public Button Button;
    public MenuOptionScreen MenuOptionScreen;
}