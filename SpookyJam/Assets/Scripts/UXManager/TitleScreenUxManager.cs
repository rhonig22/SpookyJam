using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenUxManager : MonoBehaviour
{
    [SerializeField] GameObject firstButton;

    private void Start()
    {
        if (firstButton != null)
            StartCoroutine(ForceSelectAfterFrame());
    }

    IEnumerator ForceSelectAfterFrame()
    {
        yield return null; // Wait one frame

        EventSystem.current.SetSelectedGameObject(null); // Clear previous selection
        EventSystem.current.SetSelectedGameObject(firstButton);

        // Optional: Highlight manually (WebGL sometimes skips visual states)
        var selectable = firstButton.GetComponent<Selectable>();
        if (selectable != null)
            selectable.OnSelect(null);
    }

    public void StartButtonPressed()
    {
        GameManager.Instance.LoadWorldMenu();
    }

    public void SettingsPressed()
    {
        GameManager.Instance.LoadSettings();
    }

    public void LoadWorldPressed(int world)
    {
        GameManager.Instance.LoadLevelMenuForWorld(world);
    }

    public void MainMenuClicked()
    {
        GameManager.Instance.LoadTitleScreen();
    }

    public void BackToWorldMenu()
    {
        GameManager.Instance.LoadWorldMenu();
    }
}
