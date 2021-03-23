using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button ResumeButton, SaveButton, QuitButton;

    private void Start() {
        ResumeButton.onClick.AddListener(HandleResumeClicked);
        SaveButton.onClick.AddListener(HandleSaveClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);
    }
    void HandleResumeClicked() {
        GameManager.Instance.TogglePause();
    }
    void HandleSaveClicked() {
        GameManager.Instance.SaveGame();
    }
    void HandleQuitClicked() {
        GameManager.Instance.QuitGame();
    }
}
