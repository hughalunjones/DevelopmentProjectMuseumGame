using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button ResumeButton, SaveButton, LoadButton, QuitButton;

    private void Start() {
        ResumeButton.onClick.AddListener(HandleResumeClicked);
        SaveButton.onClick.AddListener(HandleSaveClicked);
        LoadButton.onClick.AddListener(HandleLoadClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);
    }
    void HandleResumeClicked() {
        GameManager.Instance.TogglePause();
    }
    void HandleSaveClicked() {
        Events.OnSaveInitiated();
    }
    void HandleLoadClicked() {
        Events.OnLoadInitiated();
    }
    void HandleQuitClicked() {
        Events.OnSaveInitiated();
        GameManager.Instance.QuitGame();
    }
}
