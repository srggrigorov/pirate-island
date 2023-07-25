using UnityEngine;
using UnityEngine.UI;

public class MenuOptionScreen : MonoBehaviour
{
    [HideInInspector] public MainMenu MainMenu;

    [SerializeField] private Button _backButton;

    protected virtual void HandleBackButtonClick()
    {
        MainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        _backButton?.onClick.AddListener(HandleBackButtonClick);
    }

    protected virtual void OnDisable()
    {
        _backButton?.onClick.RemoveListener(HandleBackButtonClick);
    }
}