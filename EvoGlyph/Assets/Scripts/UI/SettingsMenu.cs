using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{
    public static bool IsPaused;
    [SerializeField] GameObject settingsMenu;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleSettingsMenu();
        }
    }

    private void ToggleSettingsMenu()
    {
        if (settingsMenu == null) return;

        settingsMenu.SetActive(!settingsMenu.activeSelf);
        if (settingsMenu.activeSelf)
        {
            Time.timeScale = 0f;
            IsPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            IsPaused = false;
        }
    }

    public void OnReturnToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        GameSceneManager.Instance.LoadScene("MainMenu");
    }
}
